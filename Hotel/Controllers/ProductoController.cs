using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Hotel.Models;

namespace Hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;
        public IConfiguration Configuration { get; }
        public ProductoController(IConfiguration configuration)
        {
            Configuration = configuration;
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            _productoService = new ProductoService(connectionString);
        }
        // GET: api/Producto
        [HttpGet]
        public IEnumerable<ProductoViewModel> Gets()
        {
            var productos = _productoService.ConsultarTodos().Select(p=> new ProductoViewModel(p));
            return productos;
        }

        // GET: api/Producto/5
        [HttpGet("{IdProducto}")]
        public ActionResult<ProductoViewModel> Get(string IdProducto)
        {
            var producto = _productoService.BuscarxIdentificacion(IdProducto);
            if (producto == null) return NotFound();
            var productoViewModel = new ProductoViewModel(producto);
            return productoViewModel;
        }

        // POST: api/Producto
        [HttpPost]
        public ActionResult<ProductoViewModel> Post(ProductoInputModel productoInput)
        {
            Producto producto = MapearProducto(productoInput);
            var response = _productoService.Guardar(producto);
            if (response.Error) 
            {
                ModelState.AddModelError("Guardar producto", response.Mensaje);
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                };
                return BadRequest(response.Mensaje);
            }
            return Ok(response.Producto);
        }
        
        // DELETE: api/Producto/5
        [HttpDelete("{IdProducto}")]
        public ActionResult<string> Delete(string IdProducto)
        {
            string mensaje = _productoService.Eliminar(IdProducto);
            return Ok(mensaje);
        }

        private Producto MapearProducto(ProductoInputModel productoInput)
        {
            var producto = new Producto
            {
                IdProducto = productoInput.IdProducto,
                Nombre = productoInput.Nombre,
                Tipo = productoInput.Tipo,
                Precio = productoInput.Precio,
            };
            return producto;
        }
        
        // PUT: api/Producto/5
        [HttpPut("{IdProducto}")]
        public ActionResult<ProductoViewModel> Put(string IdProducto, ProductoInputModel productoInput)
        {
            Producto producto = MapearProducto(productoInput);
            var id=_productoService.BuscarxIdentificacion(producto.IdProducto);
            if(id==null){
                return BadRequest("No encontrado");
            }else
            {
                var response = _productoService.Modificar(producto);
                if (response.Error) 
                {
                    ModelState.AddModelError("Modificar Producto", response.Mensaje);
                    var problemDetails = new ValidationProblemDetails(ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                    };
                    return BadRequest(response.Mensaje);
                }
                return Ok(response.Producto);                
            }
        }
    }
}