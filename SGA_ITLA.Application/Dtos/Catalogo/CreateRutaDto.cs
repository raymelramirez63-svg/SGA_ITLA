using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Catalogo
{
    public class CreateRutaDto
    {
        [Required(ErrorMessage = "El nombre de la ruta es obligatorio.")]
        public string NombreRuta { get; set; } = string.Empty;
    }
}