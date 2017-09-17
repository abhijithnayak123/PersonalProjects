using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Common.Util
{
    public class NexxoCard
    {

        public enum CardTypes : int
        {
            Unknown = 0,
            MaestroDebitCard = 1,
            DebitMasterCard = 2,
            MembershipCard = 3,
            ExoNexxoCard = 4,
            CardlessPAN = 5,
            PartnerCard = 6
        }

        public enum CardPhases : int
        {
            Unknown = 0,
            SinglePurseMaestro = 1,
            MultiPurseMaestro = 3,
            MultiPursePPMC = 4,
            GreenMaestro = 5,
            GreenECTRecipient = 6,
            GoldECTRecipient = 7
        }

        private static long[] MembershipCardBIN = new long[] { 6278989900000000, 6278989000000000 };

        public static CardTypes GetCardType(long CardNumber)
        {
            if (CardNumber >= 6278980000000000)
            {
                if (CardNumber < 6278988999999999)
                    return CardTypes.MaestroDebitCard;
                if (CardNumber < 6278989999999999)
                    return CardTypes.MembershipCard;
            }
            if (CardNumber >= 5151340000000000 && CardNumber < 5151349999999999)
                return CardTypes.DebitMasterCard;

            if (CardNumber >= 1000000000000000 && CardNumber < 1000009999999999)
                return CardTypes.MembershipCard;

            if (CardNumber >= 1000000 && CardNumber < 9999999999)
                return CardTypes.PartnerCard;

            if (ISOCard.IsValidCardNumber(CardNumber))
                return CardTypes.ExoNexxoCard;

            return CardTypes.Unknown;
        }

        public static bool IsMembershipCard(long CardNumber)
        {
            return IsMembershipCard(GetCardType(CardNumber));
        }

        public static bool IsMembershipCard(CardTypes cardType)
        {
            return cardType == CardTypes.MembershipCard;
        }

        public static bool IsFlaggedCard(long CardNumber)
        {
            CardTypes ThisCardType = GetCardType(CardNumber);
            return (ThisCardType == CardTypes.DebitMasterCard || ThisCardType == CardTypes.MaestroDebitCard);
        }

        public static long MinCardNumber(CardTypes CardType)
        {
            switch (CardType)
            {
                case CardTypes.MaestroDebitCard:
                    return 6278980000000000;

                case CardTypes.DebitMasterCard:
                    return 5151340000000000;

                case CardTypes.MembershipCard:
                    return 6278989000000000;

                case CardTypes.CardlessPAN:
                    return 1000000000000000;
            }
            throw new Exception("Invalid card type.");
        }

        public static long MaxCardNumber(CardTypes CardType)
        {
            switch (CardType)
            {
                case CardTypes.MaestroDebitCard:
                    return 6278980099999999;

                case CardTypes.DebitMasterCard:
                    return 5151349999999999;

                case CardTypes.MembershipCard:
                    return 6278989999999999;

                case CardTypes.CardlessPAN:
                    return 1000000999999999;
            }
            throw new Exception("Invalid card type.");
        }

        public static string ProgramName(CardTypes CardType, bool bIsEnglish)
        {
            switch (CardType)
            {
                case CardTypes.MaestroDebitCard:
                    if (bIsEnglish)
						return "MoneyGram™ Maestro® Card";
					return "Tarjeta MoneyGram™ Maestro®";

                case CardTypes.DebitMasterCard:
                    if (bIsEnglish)
						return "MoneyGram™ Prepaid MasterCard® Card";
					return "Tarjeta Prepagada MoneyGram™ MasterCard®";

                case CardTypes.MembershipCard:
                    if (bIsEnglish)
						return "MoneyGram™ Identification Card";
					return "Tarjeta MoneyGram™";
                case CardTypes.PartnerCard:
                    if (bIsEnglish)
                        return "Partner Card";
                    return "Tarjeta Compañera";
            }
            throw new Exception("Invalid card type.");
        }

        public static string ProgramName(long CardNumber, bool bIsEnglish)
        {
            return ProgramName(NexxoCard.GetCardType(CardNumber), bIsEnglish);
        }

        public static bool IsNexxoCard(long CardNumber)
        {
            NexxoCard.CardTypes CardType = GetCardType(CardNumber);
            if (CardType == CardTypes.ExoNexxoCard || CardType == CardTypes.Unknown)
                return false;
            // The BIN is Nexxo, confirm checksum
            return ISOCard.IsValidCardNumber(CardNumber);
        }

        public static bool IsPartnerCard(long CardNumber)
        {
            return GetCardType(CardNumber) == CardTypes.PartnerCard;
        }

        private static string BINCode(long CardNumber)
        {
            return BINCode(GetCardType(CardNumber));
        }

        private static string BINCode(CardTypes cardType)
        {
            switch (cardType)
            {
                case CardTypes.MaestroDebitCard:
                    return "A";

                case CardTypes.DebitMasterCard:
                    return "B";

                case CardTypes.MembershipCard:
                    return "NIC";

                case CardTypes.PartnerCard:
                    return "CPC";
            }
            throw new Exception("Not a Nexxo card: " + cardType.ToString());
        }

        public static string EncodeCardNumber(long CardNumber, bool EncodeBIN)
        {
            if (CardNumber == 0)
                return new string('0', 16);
            string sCardNumber = CardNumber.ToString();
            if (IsPartnerCard(CardNumber))
                return string.Format("CPC{0}", CardNumber);
            string encodedSuffix = "*****" + sCardNumber.Substring(sCardNumber.Length - 8);
            string sBINCode = string.Empty;
            if (IsMembershipCard(CardNumber) && sCardNumber.StartsWith("1000"))
				sBINCode = "AlloyID";
            else
            {
                try
                {
                    sBINCode = BINCode(CardNumber);
                }
				catch (Exception ex)
				{
					NLoggerCommon NLogger = new NLoggerCommon();
					NLogger.Error(string.Format("Error in Encodeing card number: {0} Stack Trace: {1}", ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available"));
				}
            }
            return (EncodeBIN ? sBINCode : string.Empty) + encodedSuffix;
        }

        public static string EncodeCardNumber(long CardNumber)
        {
            return EncodeCardNumber(CardNumber, true);
        }

        public static string SemiEncodeCardNumber(long CardNumber)
        {
            string sCardNumber = CardNumber.ToString();
            CardTypes cardType = GetCardType(CardNumber);
            if (cardType == CardTypes.ExoNexxoCard)
                return string.Format("DBT{0}", CardNumber.ToString().Substring(12));
            if (IsPartnerCard(CardNumber))
                return string.Format("PTNR{0}", CardNumber);
			string sBINCode = IsMembershipCard(cardType) && sCardNumber.StartsWith("1000") ? "AlloyID" : BINCode(cardType);
            if (sCardNumber.Length == 16)
                return sBINCode + sCardNumber.Substring(6);
            return sBINCode + sCardNumber.Substring(2);
        }

        public static string EncodeCardNumberForCustomer(long CardNumber)
        {
            string sCardNumber = CardNumber == 0 ? new string('0', 16) : CardNumber.ToString();
            string sBINCode = string.Empty;
            try
            {
                sBINCode = BINCode(CardNumber);
            }
			catch (Exception ex)
			{
				NLoggerCommon NLogger = new NLoggerCommon();
				NLogger.Error(string .Format("Error in Encodeing card number: {0} Stack Trace: {1}", ex.Message, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available"));
			}
            return sBINCode + "*****" + sCardNumber.Substring(sCardNumber.Length - 5);
        }

        public static bool GetMembershipCardNumber(long last8, out long cardnumber)
        {
            foreach (long BIN in MembershipCardBIN)
            {
                cardnumber = BIN + last8;
                if (ISOCard.IsValidCardNumber(cardnumber))
                    return true;
            }
            cardnumber = long.MinValue;
            return false;
        }
    }
}
