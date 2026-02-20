using Spectre.Console.Cli;
using System.ComponentModel;
using static Primitive.Cli.RootCommand;

namespace Primitive.Cli
{
    public class Settings : CommandSettings
    {
        [ResourceDescription("DescriptionInputPath")]
        [CommandArgument(0, "<InputPath>")]
        public required string InputPath { get; set; }

        [ResourceDescription("DescriptionOutputPath")]
        [CommandArgument(1, "<OutputPath>")]
        public required string OutputPath { get; set; }

        [ResourceDescription("DescriptionShape")]
        [CommandArgument(2, "<Shape>")]
        public required string Shape { get; set; }

        [ResourceDescription("DescriptionIterations")]
        [CommandOption("--iterations")]
        [DefaultValue(200)]
        public int Iterations { get; set; }

        [ResourceDescription("DescriptionTrials")]
        [CommandOption("--trials")]
        [DefaultValue(200)]
        public int Trials { get; set; }

        [ResourceDescription("DescriptionFailures")]
        [CommandOption("--failures")]
        [DefaultValue(30)]
        public int Failures { get; set; }

        [ResourceDescription("DescriptionBackground")]
        [CommandOption("--background")]
        [DefaultValue("")]
        public required string Background { get; set; }

        [ResourceDescription("DescriptionComputationSize")]
        [CommandOption("--computation-size")]
        [DefaultValue(256)]
        public int ComputationSize { get; set; }

        [ResourceDescription("DescriptionOutputSize")]
        [CommandOption("--output-size")]
        [DefaultValue(512)]
        public int OutputSize { get; set; }
    }
}
