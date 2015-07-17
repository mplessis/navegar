// <copyright file="TypeExtensions.cs" company="Kopigi">
// Copyright © Kopigi 2013
// </copyright>
//  ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>26/10/2013</date>
// <project>Navegar.Win8</project>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace Navegar.Libs.Class
{
    public static class TypeExtensions
    {
        public static MethodInfo GetMethod(this Type type, string methodName, object[] parameters)
        {
            //Recherche dans l'instance de l'objet
            var results = from m in type.GetTypeInfo().DeclaredMethods
                            where m.Name == methodName
                            select m;

            var methodInfos = results as List<MethodInfo> ?? results.ToList();
            if (!methodInfos.Any() && type.GetTypeInfo().BaseType != null)
            {
                //Recherche dans la classe parente
                var resultsInherited = from m in type.GetTypeInfo().BaseType.GetTypeInfo().DeclaredMethods
                                    where m.Name == methodName
                                    select m;

                methodInfos = resultsInherited as List<MethodInfo> ?? resultsInherited.ToList();
            }
            return parameters != null ? FilterMethodByParams(methodInfos, parameters) : methodInfos.FirstOrDefault();
        }

        /// <summary>
        /// Permet de filtrer sur les parametres d'une fonction pour les fonctions surchargées
        /// </summary>
        /// <param name="methods"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static MethodInfo FilterMethodByParams(IEnumerable<MethodInfo> methods,  IEnumerable<object> parameters)
        {
            if (parameters != null)
            {
                var paramsNull = parameters.Where(p => p.Equals(null));
                if (!paramsNull.Any())
                {
                    IEnumerable<Type> types = parameters.Select(param => param.GetType());
                    var methodsFiltered = methods.Where(m => m.GetParameters().Count() == parameters.Count());
                    if (methodsFiltered.Any())
                    {
                        foreach (var methodInfo in methodsFiltered)
                        {
                            if (types.SequenceEqual(methodInfo.GetParameters().Select(p => p.ParameterType).ToList()))
                            {
                                return methodInfo;
                            }
                        }
                    }
                }
                else
                {
                    var methodsFiltered = methods.Where(m => m.GetParameters().Count() == parameters.Count());
                    if (methodsFiltered.Any())
                    {
                        return methodsFiltered.First();
                    }
                }
            }
            return null;
        }
    }
}
