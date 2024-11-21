using System;
using System.Collections.Generic;

namespace ComputerApi.Models;

public partial class Osystem
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual ICollection<Comp> Comps { get; set; } = new List<Comp>();
}
