using Ciandt.Retail.MCP.Interfaces.Repositories;
using Ciandt.Retail.MCP.Models;
using System;

namespace Ciandt.Retail.MCP.Repositories;

public class PaymentFakeRepository : IPaymentRepository
{
    public async Task<ICollection<PaymentMethod>> GetPaymentMethodsAsync(string userId)
    {
        return Seed();
    }

    public static List<PaymentMethod> Seed()
    {
        return new List<PaymentMethod>
            {
                // Cartões de crédito
                new PaymentMethod
                {
                    Id = "visa_credit",
                    Name = "Visa",
                    Type = "Credit",
                    Description = "Cartão de crédito Visa",
                    SupportsInstallments = true,
                    MaxInstallments = 12,
                    HandlingFee = 0.029m,
                    IconUrl = "/assets/payment-icons/visa.svg"
                },
                new PaymentMethod
                {
                    Id = "mastercard_credit",
                    Name = "Mastercard",
                    Type = "Credit",
                    Description = "Cartão de crédito Mastercard",
                    SupportsInstallments = true,
                    MaxInstallments = 12,
                    HandlingFee = 0.031m,
                    IconUrl = "/assets/payment-icons/mastercard.svg"
                },
                new PaymentMethod
                {
                    Id = "amex_credit",
                    Name = "American Express",
                    Type = "Credit",
                    Description = "Cartão de crédito American Express",
                    SupportsInstallments = true,
                    MaxInstallments = 10,
                    HandlingFee = 0.034m,
                    IconUrl = "/assets/payment-icons/amex.svg"
                },
                
                // Cartões de débito
                new PaymentMethod
                {
                    Id = "visa_debit",
                    Name = "Visa Débito",
                    Type = "Debit",
                    Description = "Cartão de débito Visa",
                    SupportsInstallments = false,
                    HandlingFee = 0.019m,
                    IconUrl = "/assets/payment-icons/visa-debit.svg"
                },
                new PaymentMethod
                {
                    Id = "mastercard_debit",
                    Name = "Mastercard Débito",
                    Type = "Debit",
                    Description = "Cartão de débito Mastercard",
                    SupportsInstallments = false,
                    HandlingFee = 0.018m,
                    IconUrl = "/assets/payment-icons/mastercard-debit.svg"
                },
                
                // Carteiras digitais
                new PaymentMethod
                {
                    Id = "paypal",
                    Name = "PayPal",
                    Type = "PayPal",
                    Description = "Pagamento via PayPal",
                    SupportsInstallments = false,
                    HandlingFee = 0.039m,
                    IconUrl = "/assets/payment-icons/paypal.svg"
                },
                new PaymentMethod
                {
                    Id = "pix",
                    Name = "PIX",
                    Type = "Instant",
                    Description = "Pagamento instantâneo via PIX",
                    SupportsInstallments = false,
                    HandlingFee = 0.01m,
                    IconUrl = "/assets/payment-icons/pix.svg"
                },
                
                // Métodos de boleto e transferência
                new PaymentMethod
                {
                    Id = "boleto",
                    Name = "Boleto Bancário",
                    Type = "Boleto",
                    Description = "Pagamento via boleto bancário",
                    SupportsInstallments = false,
                    HandlingFee = 0.01m,
                    IconUrl = "/assets/payment-icons/boleto.svg"
                },
                new PaymentMethod
                {
                    Id = "bank_transfer",
                    Name = "Transferência Bancária",
                    Type = "BankTransfer",
                    Description = "Pagamento via transferência bancária",
                    SupportsInstallments = false,
                    HandlingFee = 0.005m,
                    IconUrl = "/assets/payment-icons/bank-transfer.svg"
                },
                
                // Métodos internacionais
                new PaymentMethod
                {
                    Id = "apple_pay",
                    Name = "Apple Pay",
                    Type = "DigitalWallet",
                    Description = "Pagamento via Apple Pay",
                    SupportsInstallments = false,
                    HandlingFee = 0.025m,
                    IconUrl = "/assets/payment-icons/apple-pay.svg"
                },
                new PaymentMethod
                {
                    Id = "google_pay",
                    Name = "Google Pay",
                    Type = "DigitalWallet",
                    Description = "Pagamento via Google Pay",
                    SupportsInstallments = false,
                    HandlingFee = 0.025m,
                    IconUrl = "/assets/payment-icons/google-pay.svg"
                }
            };
    }
}
