// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen.exe, version 0.9.0.0
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace org.apache.avro.test
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	/// <summary>
	/// * Licensed to the Apache Software Foundation (ASF) under one\r\n * or more contributor license agreements.  See the NOTICE file\r\n * distributed with this work for additional information\r\n * regarding copyright ownership.  The ASF licenses this file\r\n * to you under the Apache License, Version 2.0 (the\r\n * \"License\"); you may not use this file except in compliance\r\n * with the License.  You may obtain a copy of the License at\r\n *\r\n *     http://www.apache.org/licenses/LICENSE-2.0\r\n *\r\n * Unless required by applicable law or agreed to in writing, software\r\n * distributed under the License is distributed on an \"AS IS\" BASIS,\r\n * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\r\n * See the License for the specific language governing permissions and\r\n * limitations under the License.
	/// </summary>
	public abstract class AllCallback : All
	{
		public abstract void echo(org.apache.avro.test.AllTestRecord allTest, Avro.IO.ICallback<org.apache.avro.test.AllTestRecord> callback);
		public abstract void echoParameters(bool booleanTest, int intTest, long longTest, float floatTest, double doubleTest, byte[] bytesTest, string stringTest, org.apache.avro.test.AllEnum enumTest, org.apache.avro.test.FixedTest fixedTest, IList<System.Int64> arrayTest, IDictionary<string,System.Int64> mapTest, org.apache.avro.test.AllTestRecord nestedTest, Avro.IO.ICallback<org.apache.avro.test.AllTestRecord> callback);
	}
}
