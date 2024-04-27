using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinioConverter.UI
{
    interface UIHandler
    {
        public Task Handle(params string[] args);
    }
}
