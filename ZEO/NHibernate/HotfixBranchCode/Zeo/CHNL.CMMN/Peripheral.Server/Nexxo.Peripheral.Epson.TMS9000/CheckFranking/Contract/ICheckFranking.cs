using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MGI.Peripheral.CheckFranking.Contract
{
    public interface ICheckFranking
    {
        CheckFrankingError FrankCheck(CheckFrankData frankData);
    }
}
