using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.CartRepository;

namespace SWP391.BLL.Services.CartServices
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;

        public CartService(CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task AddProductToCartAsync(int userId, int productId, int quantity)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be provided.");
            }

            await _cartRepository.AddToCartAsync(userId, productId, quantity);
        }

        public async Task<List<OrderDetail>> GetCartDetailsAsync(int userId)
        {
            return await _cartRepository.GetCartDetailsAsync(userId);
        }

        public async Task IncreaseQuantityAsync(int userId, int productId, int quantityToAdd)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be provided.");
            }

            if (productId <= 0)
            {
                throw new ArgumentException("Product ID must be provided.");
            }

            if (quantityToAdd <= 0)
            {
                throw new ArgumentException("Quantity to add must be greater than zero.");
            }

            await _cartRepository.IncreaseQuantityAsync(userId, productId, quantityToAdd);
        }

        public async Task DecreaseQuantityAsync(int userId, int productId, int quantityToSubtract)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be provided.");
            }

            if (productId <= 0)
            {
                throw new ArgumentException("Product ID must be provided.");
            }

            if (quantityToSubtract <= 0)
            {
                throw new ArgumentException("Quantity to subtract must be greater than zero.");
            }

            await _cartRepository.DecreaseQuantityAsync(userId, productId, quantityToSubtract);
        }

        public async Task DeleteProductFromCartAsync(int userId, int productId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be provided.");
            }

            if (productId <= 0)
            {
                throw new ArgumentException("Product ID must be provided.");
            }

            await _cartRepository.DeleteProductFromCartAsync(userId, productId);
        }
    }
}
