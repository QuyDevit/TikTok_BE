using MediatR;
using TiktokBackend.Application.Common;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Users
{
    public record AddUserCommand(User User) : IRequest<ServiceResponse<User>>;
    public record UpdateUserCommand(Guid UserId,User User) : IRequest<ServiceResponse<bool>>;
    public record DeleteUserCommand(Guid UserId) : IRequest<ServiceResponse<bool>>;
    public class AddUserCommandHandler(IUserRepository userRepository) : IRequestHandler<AddUserCommand, ServiceResponse<User>>
    {
        public async Task<ServiceResponse<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = await userRepository.AddUserAsync(request.User);
            return ServiceResponse<User>.Ok(newUser, "Thêm người dùng thành công!");
        }
    }
    public class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var isUpdated = await userRepository.UpdateUserAsync(request.UserId, request.User);
            if (!isUpdated) {
                return ServiceResponse<bool>.Fail("Không tìm thấy người dùng!");
            }
            return ServiceResponse<bool>.Ok(true, "Cập nhật thông tin thành công!");
        }
    }
    public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await userRepository.DeleteUserAsync(request.UserId);
            if (!isDeleted)
                return ServiceResponse<bool>.Fail("Không tìm thấy người dùng!");

            return ServiceResponse<bool>.Ok(true, "Xóa người dùng thành công!");
        }
    }
}
