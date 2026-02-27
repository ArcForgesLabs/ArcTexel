using BenchmarkDotNet.Running;
using ArcTexel.Parser.Benchmarks.Benchmarks;

//
// var marks = new Benchmarks();
//
// marks.Setup();

BenchmarkRunner.Run<Benchmarks>();
