// -----------------------------------------------------------------------
// <copyright file="ChequeImage.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Cxn.Check.Chexar.Data
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckImage 
    {
		public virtual Guid rowguid { get; set; }

        public virtual byte[] Front { get; set; }

        public virtual byte[] Back { get; set; }

        public virtual string Format { get; set; }

        public virtual byte[] FrontTIF { get; set; }

        public virtual byte[] BackTIF { get; set; }

        public virtual ChexarTrx Trx { get; set; } // uni-directional. Don't want to load image when getting trx.

        public virtual DateTime DTTerminalCreate { get; set; }
        public virtual Nullable<DateTime> DTTerminalLastModified { get; set; }
		public virtual DateTime DTServerCreate { get; set; }
		public virtual Nullable<DateTime> DTServerLastModified { get; set; }
    }
}
