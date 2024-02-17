using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ClassLibrary1.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class TestGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            // define the execution pipeline here via a series of transformations:

            // find all additional files that end with .txt
            IncrementalValuesProvider<AdditionalText> textFiles =
                initContext.AdditionalTextsProvider;

            // read their contents and save their name
            IncrementalValuesProvider<(string name, string content)> namesAndContents =
                textFiles.Select(
                    (text, cancellationToken) =>
                        (
                            name: Path.GetFileNameWithoutExtension(text.Path),
                            content: text.GetText(cancellationToken)!.ToString()
                        )
                );

            // generate a class that contains their values as const strings
            initContext.RegisterPostInitializationOutput(spc =>
            {
                spc.AddSource(
                    $"Test2.cs",
                    $@"
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{{
    internal class Test
    {{
    }}
}}
"
                );
            });
        }
    }
}
