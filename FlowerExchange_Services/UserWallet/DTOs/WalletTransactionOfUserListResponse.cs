﻿using Domain.Entities;

namespace Application.UserWallet.DTOs;

public class WalletTransactionOfUserListResponse
{
    public Guid Id { get; set; }
    
    public string Type { get; set; }
    
    public double Amount { get; set; }
    
    public string Direction { get; set; }
    
    public string Status { get; set; }
    
    public Guid FromWallet { get; set; }
    
    
    public string? FromUserFullName { get; set; }
    
    public Guid ToWallet { get; set; }  
    
    
    public string? ToUserFullName { get; set; }
}