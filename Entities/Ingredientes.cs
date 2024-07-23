using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CursoAsp.Entities;

public class Ingredientes
{
    [Key]
    //para informar para o EF que ele é uma chave primaria
    public int Id { get; set; }

    [Required]
    //para informar que o atributo nome não pode ser nula
    [MaxLength(100)]
    //informa que o maximo que pode digitar é 100 caracteres
    public required string Nome { get; set; }
    // faz com que a String name seja obrigatoria a conter um valor

    public ICollection<Rango> Rangos { get; set; } = [];
    // vai fazer a função de mapear todos os "rangos" que usam o ingrediente
    public Ingredientes()
    {
            
    }
    [SetsRequiredMembers]
    // faz com que todos os componentes abaixo seja obrigatorio conter algum valor
    public Ingredientes(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}
