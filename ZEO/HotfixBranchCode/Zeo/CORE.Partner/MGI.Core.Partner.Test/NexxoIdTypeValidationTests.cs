using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGI.Common.DataAccess.Contract;
using System.Text.RegularExpressions;
using NUnit.Framework;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Impl;

namespace MGI.Core.Partner.Test
{
    [TestFixture]
    public class NexxoIdTypeValidationTests: AbstractPartnerTest
    {
        private IRepository<NexxoIdType> _nexxoIdTypesRepo;
        public IRepository<NexxoIdType> NexxoIdTypesRepo { set { _nexxoIdTypesRepo = value; } }

        private NexxoIdType id;
        [Test]
        public void ValidatePassportRegex()
        {
            // All countries Passport regex: ^\w{4,15}$ 
            // (Allowed Characters: Alphabets, Numbers, Underscore; Length: Between 4 to 15 characters)
            GetIdType("PASSPORT", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-4758463", id.Mask));
        }

        [Test]
        public void ValidateDriversLicenseRegex()
        {
            // US - California License regex: ^[a-zA-Z]\d{7}$ 
            // (One Alphabet, followed by exactly 7 digits)
            GetIdType("DRIVER'S LICENSE", "UNITED STATES","CALIFORNIA");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G11111111", id.Mask));
            Assert.IsFalse(Regex.IsMatch("ABCD1234", id.Mask));
            Assert.IsFalse(Regex.IsMatch("L-2843746", id.Mask));

            // US - New York License regex: ^\d{3} ?\d{3} ?\d{3}$ 
            // (### ### ### [Three digits, space, three digits, space, three digits])
            GetIdType("DRIVER'S LICENSE", "UNITED STATES", "NEW YORK");
            Assert.IsTrue(Regex.IsMatch("111 222 333", id.Mask));
            Assert.IsTrue(Regex.IsMatch("444555666", id.Mask));
            Assert.IsFalse(Regex.IsMatch("A12344321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("284-374-346", id.Mask));

            // US - Washington License regex: ^[a-zA-Z*]{7}[\w*]{5}$ 
            // (Exactly 7 characters [Alphabets or Asterisk] followed by exactly 5 characters [Alphabets, Numbers or Asterisk])
            GetIdType("DRIVER'S LICENSE", "UNITED STATES", "WASHINGTON");
            Assert.IsTrue(Regex.IsMatch("ABCD*EFW23*4", id.Mask));
            Assert.IsFalse(Regex.IsMatch("ABCDEF12345", id.Mask));
            Assert.IsFalse(Regex.IsMatch("AAAAAAA1111", id.Mask));
            Assert.IsFalse(Regex.IsMatch("ABCDEF#1_234", id.Mask));

            // US - All other States License regex: ^[\w-*]{4,15}$ 
            // (Allowed Characters: Alphabets, Numbers, Underscore, Hyphen and Asterisk; Length: Between 4 to 15 characters)
            GetIdType("DRIVER'S LICENSE", "UNITED STATES", "MAINE");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G#4758463", id.Mask));

            // Colombia - License regex: ^\d{5}-?\d{7}$
            // (#####-####### [5 digits, Hyphen, 7 digits])
            GetIdType("LICENCIA DE CONDUCCION", "COLOMBIA");
            Assert.IsTrue(Regex.IsMatch("12345-1234567", id.Mask));
            Assert.IsFalse(Regex.IsMatch("A1234-B123456", id.Mask));
            Assert.IsFalse(Regex.IsMatch("55555", id.Mask));
            Assert.IsFalse(Regex.IsMatch("1234$-1234567", id.Mask));

            // All other countries - License regex: ^\w{4,15}$
            // (Allowed Characters: Alphabets, Numbers, Underscore; Length: Between 4 to 15 characters)
            GetIdType("LICENCIA DE CONDUCCION", "VENEZUELA");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-4758463", id.Mask));
        }

        [Test]
        public void ValidateUSStateIDRegex()
        {
            // US - California State ID regex: regex: ^[a-zA-Z]\d{7}$ 
            // (One Alphabet, followed by exactly 7 digits)
            GetIdType("U.S. STATE IDENTITY CARD", "UNITED STATES", "CALIFORNIA");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G11111111", id.Mask));
            Assert.IsFalse(Regex.IsMatch("ABCD1234", id.Mask));
            Assert.IsFalse(Regex.IsMatch("L-2843746", id.Mask));

            // US - New York State ID regex: ^\d{3} ?\d{3} ?\d{3}$ 
            // (### ### ### [Three digits, space, three digits, space, three digits])
            GetIdType("U.S. STATE IDENTITY CARD", "UNITED STATES", "NEW YORK");
            Assert.IsTrue(Regex.IsMatch("111 222 333", id.Mask));
            Assert.IsTrue(Regex.IsMatch("444555666", id.Mask));
            Assert.IsFalse(Regex.IsMatch("A12344321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("284-374-346", id.Mask));

            // US - All other States State ID regex: ^[\w-*]{4,15}$ 
            // (Allowed Characters: Alphabets, Numbers, Underscore, Hyphen and Asterisk; Length: Between 4 to 15 characters)
            GetIdType("U.S. STATE IDENTITY CARD", "UNITED STATES", "MAINE");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G#4758463", id.Mask));
        }

        [Test]
        public void ValidateUS_EadGcRegex()
        {
            // US - Green Card regex: ^([aA]0\d{8}$)|^\d{7,9}$
            // (Option 1: A0 followed by exactly 8 digits; OR Option 2: Between 7 to 9 digits.)
            GetIdType("GREEN CARD / PERMANENT RESIDENT CARD", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("A012345678", id.Mask));
            Assert.IsTrue(Regex.IsMatch("2222222", id.Mask));
            Assert.IsFalse(Regex.IsMatch("54321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("AB-4321234", id.Mask));

            // US - Employment Authorization Document regex: ^([aA]0\d{8}$)|^\d{7,9}$
            // (Option 1: A0 followed by exactly 8 digits; OR Option 2: Between 7 to 9 digits.)
            GetIdType("EMPLOYMENT AUTHORIZATION CARD (EAD)", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("A012345678", id.Mask));
            Assert.IsTrue(Regex.IsMatch("222222222", id.Mask));
            Assert.IsFalse(Regex.IsMatch("1112223334", id.Mask));
            Assert.IsFalse(Regex.IsMatch("AB#4321234", id.Mask));            
        }

        [Test]
        public void ValidateUSPermitCardRegex()
        {
            // US - Work Permit Regex: ^[aA]0\d{8}$
            // (A0 followed by exactly 8 digits)
            GetIdType("WORK PERMIT", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("A012345678", id.Mask));
            Assert.IsFalse(Regex.IsMatch("A01234567", id.Mask));
            Assert.IsFalse(Regex.IsMatch("54321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("AB-4321234", id.Mask));

            // US - Re-entry Permit regex: ^[aA]0\d{8}$
            // (A0 followed by exactly 8 digits)
            GetIdType("RE-ENTRY PERMIT", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("A012345678", id.Mask));
            Assert.IsFalse(Regex.IsMatch("A01234567", id.Mask));
            Assert.IsFalse(Regex.IsMatch("54321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("AB-4321234", id.Mask));

            // US - Refugee Travel Document regex: ^[aA]0\d{8}$
            // (A0 followed by exactly 8 digits)
            GetIdType("REFUGEE TRAVEL DOCUMENT", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("A012345678", id.Mask));
            Assert.IsFalse(Regex.IsMatch("A01234567", id.Mask));
            Assert.IsFalse(Regex.IsMatch("54321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("AB-4321234", id.Mask));

            // US - Temporary Resident Card regex: ^[aA]0\d{8}$
            // (A0 followed by exactly 8 digits)
            GetIdType("TEMPORARY RESIDENT CARD", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("A012345678", id.Mask));
            Assert.IsFalse(Regex.IsMatch("A01234567", id.Mask));
            Assert.IsFalse(Regex.IsMatch("54321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("AB-4321234", id.Mask));
        }

        [Test]
        public void ValidateUSMilitaryIDRegex()
        {
            // US - Military ID regex: ^\d{9,10}$
            // (Between 9 to 10 digits)
            GetIdType("MILITARY ID", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("012345678", id.Mask));
            Assert.IsTrue(Regex.IsMatch("0123456789", id.Mask));
            Assert.IsFalse(Regex.IsMatch("54321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("43212345678", id.Mask));
            Assert.IsFalse(Regex.IsMatch("AB-4321234", id.Mask));
        }

        [Test]
        public void ValidateUSOtherCardRegex()
        {
            // US - Federal Employee ID regex: ^\w{4,15}$
            // (Allowed Characters: Alphabets, Numbers, Underscore; Length: Between 4 to 15 characters)
            GetIdType("FEDERAL EMPLOYEE ID", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-4758463", id.Mask));

            // US - NYC ID & Benefits ID regex: ^\w{4,15}$
            // (Allowed Characters: Alphabets, Numbers, Underscore; Length: Between 4 to 15 characters)
            GetIdType("NYC ID/BENEFITS ID", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-4758463", id.Mask));

            // US - San Francisco City ID regex: ^\w{4,15}$
            // (Allowed Characters: Alphabets, Numbers, Underscore; Length: Between 4 to 15 characters)
            GetIdType("SAN FRANCISCO CITY ID", "UNITED STATES");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-4758463", id.Mask));
        }

        [Test]
        public void ValidateIdentityCardRegex()
        {
            // Guatemala - Identity Card regex: ^[a-zA-Z]\d{6,7}$
            // (Exactly one alphabet, followed by 6 to 7 digits)
            GetIdType("CEDULA", "GUATEMALA");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("G654321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-4758463", id.Mask));

            // El Salvador - Identity Card regex: ^\w{9}$
            // (Allowed Characters: Alphabets, Numbers, Underscore; Length: Exactly 9 characters)
            GetIdType("DOCUMENTO UNICO DE IDENTIDAD", "EL SALVADOR");
            Assert.IsTrue(Regex.IsMatch("F12343212", id.Mask));
            Assert.IsFalse(Regex.IsMatch("1234", id.Mask));
            Assert.IsFalse(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-4758463", id.Mask));

            // Honduras - Identity Card regex: ^\w{13}$
            // (Allowed Characters: Alphabets, Numbers, Underscore; Length: Exactly 13 characters)
            GetIdType("CÉDULA/DOCUMENTO DE IDENTIDAD", "HONDURAS");
            Assert.IsTrue(Regex.IsMatch("ABCDEF1234567", id.Mask));
            Assert.IsFalse(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-#7584634357", id.Mask));

            // All other countries - Identity Card regex: ^\w{4,15}$ 
            // (Allowed Characters: Alphabets, Numbers, Underscore; Length: Between 4 to 15 characters)
            GetIdType("CÉDULA/CARTEIRA DE IDENTIDADE / REGISTRO GERAL (RG)", "BRAZIL");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-4758463", id.Mask));
        }

        [Test]
        public void ValidateConsularIDCardRegex()
        {
            // Mexico - Consular Registration Card regex: ^\d{6,9}$
            // (Between 6 to 9 digits)
            GetIdType("MATRICULA CONSULAR", "MEXICO");
            Assert.IsTrue(Regex.IsMatch("123432", id.Mask));
            Assert.IsTrue(Regex.IsMatch("987654321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G#75843", id.Mask));

            // Argentina - Consular Registration Card regex: ^\d{4,10}$
            // (Between 4 to 10 digits)
            GetIdType("MATRICULA CONSULAR / CONSULAR I.D. CARD", "ARGENTINA");
            Assert.IsTrue(Regex.IsMatch("1234567890", id.Mask));
            Assert.IsTrue(Regex.IsMatch("1234", id.Mask));
            Assert.IsFalse(Regex.IsMatch("3323442456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G4758463", id.Mask));

            // Guatemala - Consular Registration Card regex: ^[a-zA-Z]-?\d{6,7}$
            // (Exactly one alphabet, followed by zero or one Hyphen, followed by 6 to 7 digits)
            GetIdType("TARJETA DE IDENTIFICACIÓN CONSULAR", "GUATEMALA");
            Assert.IsTrue(Regex.IsMatch("F-1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("G654321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G#4758463", id.Mask));

            // All other countries - Consular Registration Card regex: ^\w{4,15}$ 
            // (Allowed Characters: Alphabets, Numbers, Underscore; Length: Between 4 to 15 characters)
            GetIdType("TARJETA DE IDENTIFICACIÓN CONSULAR", "BOLIVIA");
            Assert.IsTrue(Regex.IsMatch("F1234321", id.Mask));
            Assert.IsTrue(Regex.IsMatch("ABCDEF456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-4758463", id.Mask));
        }

        [Test]
        public void ValidateElectoralCardRegex()
        {
            // El Salvador - Electoral Card regex: ^\d{14}$
            // (Exactly 14 digits)
            GetIdType("CARNET ELECTORAL", "EL SALVADOR");
            Assert.IsTrue(Regex.IsMatch("12345671234567", id.Mask));
            Assert.IsFalse(Regex.IsMatch("123456756297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-#7584634357", id.Mask));

            // Mexico - Electoral Card regex: ^\d{12,13}$
            // (Between 12 to 13 digits)
            GetIdType("INSTITUTO FEDERAL ELECTORAL", "MEXICO");
            Assert.IsTrue(Regex.IsMatch("123456123457", id.Mask));
            Assert.IsFalse(Regex.IsMatch("`12345456297432", id.Mask));
            Assert.IsFalse(Regex.IsMatch("321", id.Mask));
            Assert.IsFalse(Regex.IsMatch("G-#7584634357", id.Mask));
        }

        private void GetIdType(string idName, string country, string state = null)
        {
            if (state == null)
                id = _nexxoIdTypesRepo.FindBy(n => n.Name == idName && n.CountryId.Name == country);
            else
                id = _nexxoIdTypesRepo.FindBy(n => n.Name == idName && n.CountryId.Name == country && n.StateId.Name == state);
        }
    }
}
