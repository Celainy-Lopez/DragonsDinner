﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDinner.Data.Models;

public class ProductosFood
{
    [Key]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "Debe ingresar un nombre")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
    [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "El nombre solo debe contener letras.")]
    public string Nombre { get; set; } = string.Empty;

    public int Existencia { get; set; }

    [Required(ErrorMessage = "Debe ingresar una descripción")]
    [StringLength(300, ErrorMessage = "Ha exedido el número de caracteres.")]
    [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "La descripción solo debe contener letras.")]
    public string Descripcion { get; set; } = string.Empty ;

    [Required(ErrorMessage = "Debe ingresar un precio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
    public double Precio { get; set; }

    public int CategoriaId { get; set; }
    [ForeignKey("CategoriaId")]
    public  Categorias? Categoria { get; set; }

    [Required(ErrorMessage = "Debe ingresar una URL")]
    public string Imagen { get; set; } = string.Empty;

    public double Costo { get; set; }

    public int Cantidad { get; set; }
}
