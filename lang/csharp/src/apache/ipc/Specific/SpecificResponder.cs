﻿/**
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using Avro.Generic;
using Avro.IO;
using Avro.Specific;
using Avro.ipc.Generic;

namespace Avro.ipc.Specific
{
    public class SpecificResponder<T> : GenericResponder
        where T : class, ISpecificProtocol
    {
        private static readonly Dictionary<string, string> BaseTypes =
            new Dictionary<string, string>
                {
                    {"string", typeof (string).FullName},
                    {"int", typeof (int).FullName},
                    {"bytes", typeof (byte[]).FullName},
                    {"float", typeof (float).FullName},
                    {"double", typeof (double).FullName},
                    {"boolean", typeof (bool).FullName},
                    {"long", typeof (long).FullName}
                };

        private readonly T impl;

        public SpecificResponder(T impl)
            : base(impl.Protocol)
        {
            this.impl = impl;
        }

        public override object Respond(Message message, object request)
        {
            int numParams = message.Request.Fields.Count;
            var parameters = new Object[numParams];
            var parameterTypes = new Type[numParams];

            int i = 0;

            foreach (Field field in message.Request.Fields)
            {
                string name = GetName(field);

                Type type = ObjectCreator.Instance.GetType(name, field.Schema.Tag);
                parameterTypes[i] = type;
                parameters[i] = ((GenericRecord) request)[field.Name];

                i++;
            }

            MethodInfo method = typeof (T).GetMethod(message.Name, parameterTypes);
            try
            {
                return method.Invoke(impl, parameters);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        private static string GetName(Field field)
        {
            var namedSchema = field.Schema as NamedSchema;
            string name = namedSchema != null ? namedSchema.Fullname : field.Schema.Name;

            if (name == "array")
            {
                name= ((((Avro.ArraySchema) (field.Schema))).ItemSchema).Name;
            }
            else if (name == "map")
            {
                name = (((Avro.MapSchema)(field.Schema)).ValueSchema).Name;
                //return string.Format("System.Collections.Generic.IDic<{0}>", ((((Avro.ArraySchema)(field.Schema))).ItemSchema).Name);
            }

            string value;
            return BaseTypes.TryGetValue(name, out value) ? value : name;
        }

        public override void WriteError(Schema schema, object error, Encoder output)
        {
            new SpecificWriter<object>(schema).Write(error, output);
        }

        public override object ReadRequest(Schema actual, Schema expected, Decoder input)
        {
            return new SpecificReader<object>(actual, expected).Read(null, input);
        }

        public override void WriteResponse(Schema schema, object response, Encoder output)
        {
            new SpecificWriter<object>(schema).Write(response, output);
        }
    }
}