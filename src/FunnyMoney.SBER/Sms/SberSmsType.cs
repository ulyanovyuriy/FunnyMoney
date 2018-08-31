﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FunnyMoney.SBER.Sms
{
    public enum SberSmsType
    {
        /// <summary>
        /// Неизвестное
        /// </summary>
        None,
        /// <summary>
        /// Списание
        /// </summary>
        WriteOff,

        /// <summary>
        /// Покупка
        /// </summary>
        Buy,

        /// <summary>
        /// Выдача в банкомате
        /// </summary>
        Atm,

        /// <summary>
        /// Зачисление зарплаты
        /// </summary>
        Salary,

        /// <summary>
        /// Зачисление
        /// </summary>
        PayIn,

        /// <summary>
        /// Оплата
        /// </summary>
        PayOut,

        /// <summary>
        /// Отмена покупки
        /// </summary>
        BuyCancel,

        /// <summary>
        /// Информационное сообщение
        /// </summary>
        Info,
    }
}