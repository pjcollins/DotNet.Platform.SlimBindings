using System;
using System.IO;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace Microsoft.Maui.BindingExtensions.Build.Tasks
{
    public class FixUpApiDefinitions : Task
    {
        public string TaskPrefix => "FXAD";

        [Required]
        public ITaskItem [] ApiDefinitionsFiles { get; set; } = Array.Empty<ITaskItem> ();

        [Required]
        public string IntermediateOutputPath { get; set; } = string.Empty;

        [Output]
        public ITaskItem [] UpdatedApiDefinitionsFiles { get; set; } = Array.Empty<ITaskItem> ();

        public FixUpApiDefinitions()
        {
        }

        public override bool Execute ()
        {
            try {
                // TODO Apply common fixes to ApiDefinitions file, validate it, and save it to intermediate out
                var apiDefinitionOutputs = new List<string> ();
                foreach (var apidef in ApiDefinitionsFiles) {
                    var updatedFileName = $"{Path.GetFileNameWithoutExtension(apidef.ItemSpec)}.g{Path.GetFileNameWithoutExtension(apidef.ItemSpec)}";
                    var updatedFileDestination = Path.Combine(IntermediateOutputPath, updatedFileName);
                    // TODO Improve incremental building
                    if (File.Exists(updatedFileDestination))
                        File.Delete(updatedFileDestination);

                    File.Copy(apidef.ItemSpec, updatedFileDestination);
                    apiDefinitionOutputs.Add(updatedFileDestination);
                }
                UpdatedApiDefinitionsFiles = apiDefinitionOutputs.Select(a => new TaskItem(a)).ToArray();
                return true;
            } catch (Exception ex) {
                Log.LogCodedError($"{TaskPrefix}0100", ex.ToString());
                return false;
            }
        }

    }
}
