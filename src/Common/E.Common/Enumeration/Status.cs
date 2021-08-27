using System;
using System.ComponentModel;

namespace E.Common.Enumeration
{
    /// <summary>
    /// <para>状态码</para>
    /// <para>规则为16进制</para>
    /// <para>      前4位为功能序号</para>
    /// <para>      后4位为错误序号</para>
    /// <para>功能码 0：系统相关</para>
    /// <para>      1：用户相关</para>
    /// </summary>
    [Flags]
    public enum Status : long
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0x00000,

        /// <summary>
        /// 未知错误
        /// </summary>
        [Description("未知错误")]
        Unknown = 0x00001,

        /// <summary>
        /// 数据库错误
        /// </summary>
        [Description("数据库错误")]
        DatabaseError = 0x00002,

        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParameterError = 0x00101,

        /// <summary>
        /// ClientID不能为空
        /// </summary>
        [Description("ClientID不能为空")]
        ClientIdIsEmpty = 0x01001,
        /// <summary>
        /// ClientSecret不能为空
        /// </summary>
        [Description("ClientSecret不能为空")]
        ClientSecretIsEmpty = 0x01002,
        /// <summary>
        /// 认证失败
        /// </summary>
        [Description("认证失败")]
        AuthenticationFailed = 0x01003,
        /// <summary>
        /// 用户名称已存在
        /// </summary>
        [Description("用户名称已存在")]
        NameExist = 0x10001,

        /// <summary>
        /// 用户名称为空
        /// </summary>
        [Description("用户名称为空")]
        NameIsEmpty = 0x10002,

        /// <summary>
        /// 密码为空
        /// </summary>
        [Description("密码为空")]
        PasswordIsEmpty = 0x10003,

        /// <summary>
        /// 用户名/密码不匹配
        /// </summary>
        [Description("用户名/密码不匹配")]
        NamePasswordError = 0x10004
    }
}
