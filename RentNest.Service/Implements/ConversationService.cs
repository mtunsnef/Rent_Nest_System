﻿using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.Implements;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Implements
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;

        public ConversationService(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<Conversation> AddIfNotExistsAsync(int senderId, int receiverId, int? postId)
        {
            return await _conversationRepository.AddIfNotExistsAsync(senderId, receiverId, postId);
        }

        public async Task<IEnumerable<Conversation>> GetAll()
        {
           return await _conversationRepository.GetAll();
        }

        public async Task<List<Conversation>> GetByUserIdAsync(int userId) 
        {
            return await _conversationRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<object>> GetConversationMessagesAsync(int conversationId, int currentUserId)
        {
            return await _conversationRepository.GetConversationMessagesAsync(conversationId, currentUserId);
        }

        public async Task<Conversation?> GetConversationWithMessagesAsync(int conversationId)
        {
            return await _conversationRepository.GetConversationWithMessagesAsync(conversationId);
        }
    }
}
