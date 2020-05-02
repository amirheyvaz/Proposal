using InfrastructureLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.AbstractModels
{
    public class TEntity<T> :  IEntity<T>
    {
        [Key]
        public virtual T ID { get; set; }
    }
}
