using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdministrationController(RoleManager<IdentityRole> roleManager,
    UserManager<IdentityUser> userManager) : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    public IActionResult CreateRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = model.RoleName
            };
            IdentityResult result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles", "Administration");
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult ListRoles()
    {
        var roles = _roleManager.Roles;
        return View(roles);
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            ViewBag.ErrorMessage = $"Role with id = {id} cannot be found";
            return NotFound();
        }
        var model = new EditRoleViewModel
        {
            Id = role.Id,
            RoleName = role.Name!
        };

        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, role.Name!))
            {
                model.Users.Add(user.UserName!);
            }
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> EditRole(EditRoleViewModel model)
    {
        var role = await _roleManager.FindByIdAsync(model.Id);
        if (role == null)
        {
            ViewBag.ErrorMessage = $"Role with id = {model.Id} cannot be found";
            return NotFound();
        }
        else
        {
            role.Name = model.RoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditUsersInRole(string roleId)
    {
        ViewBag.roleId = roleId;
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
            return NotFound();
        }
        var model = new List<UserRoleViewModel>();
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            var userRoleViewModel = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName!
            };
            if (await _userManager.IsInRoleAsync(user, role.Name!))
            {
                userRoleViewModel.IsSelected = true;
            }
            else
            {
                userRoleViewModel.IsSelected = false;
            }
            model.Add(userRoleViewModel);
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
    {
        ViewBag.roleId = roleId;
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
            return NotFound();
        }
        for (int i = 0; i < model.Count; i++)
        {
            var user = await _userManager.FindByIdAsync(model[i].UserId);
            IdentityResult result = null!;
            if (model[i].IsSelected && !await _userManager.IsInRoleAsync(user!, role.Name!))
            {
                result = await _userManager.AddToRoleAsync(user!, role.Name!);
            }
            else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user!, role.Name!))
            {
                result = await _userManager.RemoveFromRoleAsync(user!, role.Name!);
            }
            else
            {
                continue;
            }
            if(result.Succeeded)
            {
                if(i < model.Count - 1)
                    continue;
                else
                    return RedirectToAction("EditRole", new {id = roleId});
            }
        }
        return View();
    }
}