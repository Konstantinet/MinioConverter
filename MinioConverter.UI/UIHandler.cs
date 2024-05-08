using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinioConverter.UI
{
    interface UIHandler
    {
        public Task<int> Handle(IConfiguration config, params string[] args);
    }
}
