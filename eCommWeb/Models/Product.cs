using System.ComponentModel.DataAnnotations;

namespace eCommWeb.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:$#.##}", ApplyFormatInEditMode = true)]
        public float? Price { get; set; }
        public string Photo { get; set; }
    }
}
