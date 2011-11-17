/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

using wojilu.ORM;

namespace wojilu.Reflection {


    internal class ILPropertyAccessor {

        private static IDictionary _accessorList = new Hashtable();
        private static AssemblyBuilder _dynamicAssembly = null;
        private static ModuleBuilder _moduleBuilder = null;
        private static TypeBuilder _typeBuilder = null;
        private static Hashtable _typeHash = new Hashtable();

        private static void CacheAccessor( Type type ) {
            foreach (PropertyInfo info in type.GetProperties()) {
                String key = getKeyName( type.FullName, info.Name );
                CreateDynamicPropertyAccessor( type, info.Name );
                IPropertyAccessor accessor = (IPropertyAccessor)rft.GetInstance( _typeBuilder.CreateType() );
                _typeBuilder = null;
                _accessorList.Add( key, accessor );
            }
        }

        public static void Clear() {
            _dynamicAssembly = null;
            _moduleBuilder = null;
            _typeBuilder = null;
            _accessorList.Clear();
        }

        private static TypeBuilder CreateDynamicPropertyAccessor( Type _targetType, String mProperty ) {
            Hashtable hashtable = getInitTypes();
            TypeBuilder builder = getDynamicType( _targetType, mProperty );
            builder.AddInterfaceImplementation( typeof( IPropertyAccessor ) );
            Type[] parameterTypes = new Type[] { typeof( Object ) };
            Type returnType = typeof( Object );
            ILGenerator iLGenerator = builder.DefineMethod( "Get", MethodAttributes.Virtual | MethodAttributes.Public, returnType, parameterTypes ).GetILGenerator();
            MethodInfo method = _targetType.GetMethod( "get_" + mProperty );
            if (method != null) {
                iLGenerator.DeclareLocal( typeof( Object ) );
                iLGenerator.Emit( OpCodes.Ldarg_1 );
                iLGenerator.Emit( OpCodes.Castclass, _targetType );
                iLGenerator.EmitCall( OpCodes.Call, method, null );
                if (method.ReturnType.IsValueType) {
                    iLGenerator.Emit( OpCodes.Box, method.ReturnType );
                }
                iLGenerator.Emit( OpCodes.Stloc_0 );
                iLGenerator.Emit( OpCodes.Ldloc_0 );
            }
            else {
                iLGenerator.ThrowException( typeof( MissingMethodException ) );
            }
            iLGenerator.Emit( OpCodes.Ret );
            Type[] arrTypes = new Type[] { typeof( Object ), typeof( Object ) };
            Type t = null;
            ILGenerator mGenerator = builder.DefineMethod( "Set", MethodAttributes.Virtual | MethodAttributes.Public, t, arrTypes ).GetILGenerator();
            MethodInfo methodInfo = _targetType.GetMethod( "set_" + mProperty );
            if (methodInfo != null) {
                Type parameterType = methodInfo.GetParameters()[0].ParameterType;
                mGenerator.DeclareLocal( parameterType );
                mGenerator.Emit( OpCodes.Ldarg_1 );
                mGenerator.Emit( OpCodes.Castclass, _targetType );
                mGenerator.Emit( OpCodes.Ldarg_2 );
                if (parameterType.IsValueType) {
                    mGenerator.Emit( OpCodes.Unbox, parameterType );
                    if (hashtable[parameterType] != null) {
                        OpCode opcode = (OpCode)hashtable[parameterType];
                        mGenerator.Emit( opcode );
                    }
                    else {
                        mGenerator.Emit( OpCodes.Ldobj, parameterType );
                    }
                }
                else {
                    mGenerator.Emit( OpCodes.Castclass, parameterType );
                }
                mGenerator.EmitCall( OpCodes.Callvirt, methodInfo, null );
            }
            else {
                mGenerator.ThrowException( typeof( MissingMethodException ) );
            }
            mGenerator.Emit( OpCodes.Ret );
            return _typeBuilder;
        }

        public static void Get( Object target, String propertyName ) {
            GetAccessor( target.GetType().FullName, propertyName ).Get( target );
        }

        public static IPropertyAccessor GetAccessor( String typeFullName, String propertyName ) {
            String str = getKeyName( typeFullName, propertyName );
            return (getAccessorList()[str] as IPropertyAccessor);
        }

        private static IDictionary getAccessorList() {
            if (_accessorList == null) {
                Init();
            }
            return _accessorList;
        }

        private static ModuleBuilder GetDynamicModule() {
            if (_dynamicAssembly == null) {
                AssemblyName name = new AssemblyName();
                name.Name = "PropertyAccessorAssembly";
                _dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly( name, AssemblyBuilderAccess.Run );
                _moduleBuilder = _dynamicAssembly.DefineDynamicModule( "MainModule" );
            }
            return _moduleBuilder;
        }

        private static TypeBuilder getDynamicType( Type _targetType, String propertyName ) {
            if (_typeBuilder == null) {
                _typeBuilder = GetDynamicModule().DefineType( "__dynamicPropertyAccessor." + _targetType.FullName + "." + propertyName, TypeAttributes.Public );
            }
            return _typeBuilder;
        }

        private static Hashtable getInitTypes() {
            if ((_typeHash == null) || (_typeHash.Count == 0)) {
                _typeHash[typeof( sbyte )] = OpCodes.Ldind_I1;
                _typeHash[typeof( byte )] = OpCodes.Ldind_U1;
                _typeHash[typeof( char )] = OpCodes.Ldind_U2;
                _typeHash[typeof( short )] = OpCodes.Ldind_I2;
                _typeHash[typeof( ushort )] = OpCodes.Ldind_U2;
                _typeHash[typeof( int )] = OpCodes.Ldind_I4;
                _typeHash[typeof( uint )] = OpCodes.Ldind_U4;
                _typeHash[typeof( long )] = OpCodes.Ldind_I8;
                _typeHash[typeof( ulong )] = OpCodes.Ldind_I8;
                _typeHash[typeof( Boolean )] = OpCodes.Ldind_I1;
                _typeHash[typeof( double )] = OpCodes.Ldind_R8;
                _typeHash[typeof( float )] = OpCodes.Ldind_R4;
            }
            return _typeHash;
        }

        private static String getKeyName( String typeFullName, String propertyName ) {
            return (typeFullName + "_" + propertyName);
        }

        public static void Init() {
            Clear();
            foreach (DictionaryEntry entry in MappingClass.Instance.ClassList) {
                CacheAccessor( ((EntityInfo)entry.Value).Type );
            }
        }

        public static void Init( Type type ) {
            CacheAccessor( type );
        }

        public static void Set( Object target, String propertyName, Object propertyValue ) {
            GetAccessor( target.GetType().FullName, propertyName ).Set( target, propertyValue );
        }


    }
}

