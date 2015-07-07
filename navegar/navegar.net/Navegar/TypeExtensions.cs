﻿// <copyright file="TypeExtensions.cs" company="Kopigi">
// Copyright © Kopigi 2013
// </copyright>
//  ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>26/10/2013</date>
// <project>Navegar</project>
// <web>http://navegar.kopigi.fr</web>
// <license>
// The MIT License (MIT)
// 
// Copyright (c) 2013 Marc Plessis
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
//  </license>

#region using

using System.Linq;
using System.Reflection;

#endregion

namespace System
{
    public static class TypeExtensions
    {
        public static MethodInfo GetMethodToLoad(this Type type, string methodName, object[] parameters)
        {
            try
            {
                const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.ExactBinding;
                var paramsTypes = new Type[]{};
                if (parameters != null)
                {
                    if (parameters.All(p => p != null))
                    {
                        paramsTypes = parameters.Select(p => p.GetType()).ToArray();    
                    }
                }
                var method = type.GetMethod(methodName, flags, null, paramsTypes, new ParameterModifier[] {});

                if (method == null && type.BaseType != null)
                {
                    //Recherche dans la classe parente
                    var methodInherited = type.BaseType.GetMethod(methodName, flags, null, paramsTypes, new ParameterModifier[] {});
                    return methodInherited;
                }
                return method;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
