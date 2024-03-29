﻿using Budget.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Budget.Models.ViewModel.Account;

public class RegisterModel : AccountModel
{
    [RequiredEnter(nameof(Password))]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;


    [Display(Name = "Confirm Password")]
    [RequiredEnter(ErrorMessage = "Repeat " + nameof(Password))]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The passwords doesn't match")]
    public string ConfirmPassword { get; set; } = null!;
}
