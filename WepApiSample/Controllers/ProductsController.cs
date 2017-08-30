using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using WepApiSample.Exceptions;
using WepApiSample.Models;



namespace WepApiSample.Controllers
{
    // contorller 전역에 사용자 예외필터 적용[NotImplExceptionFilter]
    public class ProductsController : ApiController
    {
        
       static readonly IProductRepository repository = new ProductRepository();

        [System.Web.Http.ActionName("GetAll")]
        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();
        }

        [System.Web.Http.ActionName("GetId")]
        public List<Product> GetProduct(int id)
        {
            Product product = repository.Get(id);
            if (product == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No product with ID ={0}", id)),
                    ReasonPhrase = "Product ID Not Found"
                };
                throw new HttpResponseException(resp);
            }
            List<Product> item = new List<Product>();
            item.Add(product);
            return item;
        }

        [System.Web.Http.ActionName("GetCategory")]
        public IEnumerable<Product> GetProductsByCategory(String category)
        {           
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("Add")]
        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);

            /*CreateResponse 메서드는 HttpResponseMessage 개체를 생성하고 
             * 자동으로 응답 메시지의 본문에 직렬화된 Product 개체의 표현식*/
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("Put")]
        public void PutProduct(Product product)
        {
            
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
        
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("Delete")]
        public String DeleteProduct([FromBody]int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                //HttpError와 HttpResponseException을 동시에 사용
                var message = string.Format("Product with id = {0} not found", id);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound,message));                
            }

            return "success";
        }
    }
}