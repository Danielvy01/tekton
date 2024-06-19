using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekton.RedisCaching;

namespace Tekton.Configuration.Damain.Entities.Producto
{
    [ExcludeFromCodeCoverage]
    public class ProductoEntity : AuditoriaEntity
    {
        /// <summary>
        /// Id del producto
        /// </summary>
        public int? ProductId { get; set; }
        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Estado id del producto
        /// </summary>
        public short Status { get; set; }
        /// <summary>
        /// Estado del Producto Este campo se obtiene del caché basado e el "Status"
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        /// Cantidad real del producto
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// Descripción del producto
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Precio del producto
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// Porcentage de descuento del producto
        /// </summary>
        public decimal? Discount { get; set; }
        /// <summary>
        /// Precio final del producto con el descuento
        /// </summary>
        public decimal? FinalPrice { get; set; }

        /// <summary>
        /// Obtener el precio
        /// </summary>
        public decimal? ObtenerFinalPrice()
        {
            Discount = ObtenerDescuentoOnline();
            if (Price > 0)
                FinalPrice = Price * (Discount ?? 0 - 100) / 100;
            else
                FinalPrice = 0;
            return this.FinalPrice;
        }

        private decimal? ObtenerDescuentoOnline()
        {
            decimal? discount = -1;
            try
            {
                using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
                {
                    var fullUrl = "https://6672fa246ca902ae11b2a520.mockapi.io/api/v1/discount";


                    var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new System.Uri(fullUrl) };

                    var result = client.SendAsync(httpRequestMessage).Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var responseData = result.Content.ReadAsStringAsync().Result;
                        List<int> _discount = JsonConvert.DeserializeObject<List<int>>(responseData);
                        discount = _discount.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                discount = -1;
            }
            return discount;
        }

        /// <summary>
        /// Obtener el estatus
        /// </summary>
        public string? ObtenerElStatus(IRedisCacheService _redisCacheService)
        {
            var data = _redisCacheService.GetData<List<EstadosDTO>?>("estados-cache");
            if (data == null)
            {
                List<EstadosDTO> listaEstados = new List<EstadosDTO>();
                listaEstados.Add(new EstadosDTO() { Codigo = 1, Nombre = "Active" });
                listaEstados.Add(new EstadosDTO() { Codigo = 0, Nombre = "Inactive" });
                _redisCacheService.SetData("estados-cache", listaEstados, 5);
                data = _redisCacheService.GetData<List<EstadosDTO>?>("estados-cache");
            }
            var obj = data.ToList().FindAll(x => x.Codigo == this.Status).FirstOrDefault();
            if (obj != null)
                return obj.Nombre;
            else
                return "";
        }
    }
}
