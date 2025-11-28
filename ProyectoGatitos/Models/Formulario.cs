using System.ComponentModel.DataAnnotations;

namespace ProyectoGatitos.Models
{
    public class Formulario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor ingresá tu nombre.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Por favor ingresá tu apellido.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Por favor ingresá un número de contacto.")]
        public string NumeroContacto { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Por favor ingresá un email válido.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contanos por qué querés adoptar.")]
        public string MotivoAdopcion { get; set; }

        [Required(ErrorMessage = "Por favor compartí tus redes sociales.")]
        public string RedesSociales { get; set; }

        [Required(ErrorMessage = "Por favor ingresá tu dirección.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "La localidad es obligatoria.")]
        public string Localidad { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio.")]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "Por favor seleccioná el tipo de vivienda.")]
        public string TipoVivienda { get; set; }

        [Required(ErrorMessage = "Por favor seleccioná al menos una característica del hogar.")]
        public List<string> CaracteristicasHogar { get; set; }

        [Required(ErrorMessage = "Por favor indicá si hay redes de protección.")]
        public string RedesProteccion { get; set; }

        [Required(ErrorMessage = "Por favor indicá si tu familia está de acuerdo.")]
        public string AprobacionFamiliar { get; set; }

        [Required(ErrorMessage = "Por favor contanos qué harías si te mudás.")]
        public string QueHariasSiTeMudás { get; set; }

        [Required(ErrorMessage = "Por favor indicá si tenés mascotas.")]
        public string TieneMascotas { get; set; }

        [Required(ErrorMessage = "Por favor brindá detalles sobre tus mascotas.")]
        public string DetallesMascotas { get; set; }

        [Required(ErrorMessage = "Por favor compartí tu opinión sobre la esterilización.")]
        public string OpinionEsterilizacion { get; set; }

        [Required(ErrorMessage = "Por favor indicá si tus mascotas están castradas.")]
        public string MascotasCastradas { get; set; }

        [Required(ErrorMessage = "Por favor comentá qué medidas tomarías para la adaptación.")]
        public string MedidasAdaptacion { get; set; }

        [Required(ErrorMessage = "Por favor indicá con quién vivís.")]
        public string ConQuienVive { get; set; }

        [Required(ErrorMessage = "Por favor comentá sobre el riesgo en fiestas.")]
        public string RiesgoFiestas { get; set; }

        [Required(ErrorMessage = "Por favor indicá qué alimento pensás darle.")]
        public string MarcaAlimento { get; set; }

        [Required(ErrorMessage = "Por favor contanos qué lugar ocuparía el animal en tu vida.")]
        public string ImportanciaAnimal { get; set; }

        [Required(ErrorMessage = "Por favor indicá si estás dispuesto a trasladarte.")]
        public string DispuestoTraslado { get; set; }

        [Required(ErrorMessage = "Por favor agregá algún comentario adicional.")]
        public string ComentariosAdicionales { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public bool Revisado { get; set; } = false;
    }
}
