﻿using System;
using System.Collections.Generic;
using System.Threading;
using dnlib.DotNet;

namespace dnExplorer.Analysis {
	public class MethodUsedByAnalysis : AnalyzerAnalysis<MethodDef> {
		public MethodUsedByAnalysis(MethodDef targetMethod)
			: base(targetMethod) {
		}

		public override string Name {
			get { return "Used By"; }
		}

		public override IEnumerable<object> Run(IApp app, CancellationToken token) {
			return new ScopedAnalyzer<MethodDef>(app, Item, AnalyzeType).Run(token);
		}

		IEnumerable<MethodDef> AnalyzeType(TypeDef type) {
			var comparer = new SigComparer(SigComparerOptions.CompareMethodFieldDeclaringType);
			foreach (var method in type.Methods) {
				if (!method.HasBody)
					continue;

				foreach (var instr in method.Body.Instructions)
					if (instr.Operand is IMethod && comparer.Equals((IMethod)instr.Operand, Item)) {
						yield return method;
						break;
					}
			}
		}
	}
}