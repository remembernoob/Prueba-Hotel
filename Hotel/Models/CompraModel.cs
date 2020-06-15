using Entity;
using System.ComponentModel.DataAnnotations;
using System;

namespace Hotel.Models
{
    public class CompraInputModel
    {
        public int IdCompra { get; set; }

        public string IdProducto { get; set; }

        public DateTime FechaCompra { get; set; }

        public int CantidadProductos { get; set; }

        public decimal TotalCompra { get; set; }

        public class CompraViewModel : CompraInputModel
        {
            public CompraViewModel()
            {
            }
            public CompraViewModel(Compra compra)
            {
                IdCompra = compra.IdCompra;
                IdProducto = compra.IdProducto;
                FechaCompra = compra.FechaCompra;
                CantidadProductos = compra.CantidadProductos;
                TotalCompra = compra.TotalCompra;
            }
        }


    }
}