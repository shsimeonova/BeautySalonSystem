using BeautySalonSystem.Products.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Models.ProductOffer
{
    public class ListProductOffersModel
    {
        public ListProductOffersModel()
        {
            Offers = new List<GetOfferModel>();
        }
        public List<GetOfferModel> Offers { get; set; }
    }
}
