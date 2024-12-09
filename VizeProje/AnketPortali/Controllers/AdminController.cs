using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using AnketPortali.Repositories;
using AnketPortali.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AnketPortali.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;

        public AdminController(UserRepository userRepository, INotyfService notyfService, IMapper mapper)
        {
            _userRepository = userRepository;
            _notyfService = notyfService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UserList()
        {
            var users = await _userRepository.GetAllAsync();
            var viewModel = _mapper.Map<List<UserModel>>(users);
            return View(viewModel);
        }
        
    }
}
