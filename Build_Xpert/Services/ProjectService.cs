using Build_Xpert.Model;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace Build_Xpert.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.ProjectPhase)
                .Include(p => p.Client)
                .Include(p => p.Admin)
                .ToListAsync() ?? new List<Project>();
        }

        public async Task AddProjectAsync(Project project, string clientId, string adminId)
        {
            if (project == null || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(adminId))
                throw new ArgumentException("El proyecto, cliente o administrador no pueden ser nulos.");

            var client = await _context.Users.FindAsync(clientId);
            var admin = await _context.Users.FindAsync(adminId);

            if (client == null || admin == null)
                throw new Exception("El Cliente o Administrador no existen.");

            project.ClientId = clientId;
            project.Client = client;
            project.AdminId = adminId;
            project.Admin = admin;

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            var defaultPhases = ProjectPhaseDefaults.GenerateDefaultPhases(project.Id);
            await _context.ProjectPhases.AddRangeAsync(defaultPhases);

            // Guarda las fases
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            if (project == null)
                throw new ArgumentException("El proyecto no puede ser nulo.");

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectStatusAsync(int id, string status)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTaskToProjectPhaseAsync(int projectId, int phaseId, string taskTitle, string description, bool IsCompleted)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectPhase)
                .FirstOrDefaultAsync(p => p.Id == projectId) ?? throw new Exception("Proyecto no encontrado.");

            var phase = project.ProjectPhase.FirstOrDefault(p => p.PhaseId == phaseId) ?? throw new Exception("Fase no encontrada");

            var phaseTasks = new ProjectPhaseTasks
            {
                Title = taskTitle,
                Description = description,
                IsCompleted = IsCompleted,
                PhaseId = phaseId,
                CreatedAt = DateTime.Now
            };

            await _context.Tasks.AddAsync(phaseTasks);
            await _context.SaveChangesAsync();

        }


        public async Task ToggleTaskCompletionAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.ProjectPhase)
                    .ThenInclude(p => p.ProjectPhaseTasks)
                .Include(p => p.Client)
                .Include(p => p.Admin)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProjectPhaseTasks> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.ProjectPhase)
                .FirstOrDefaultAsync(t => t.TaskId == id);
        }

        public async Task<List<ProjectPhaseTasks>> GetTasksByProjectIdAndPhaseIdAsync(int phaseId, int projectId)
        {
            return await _context.Tasks
                .Include(t => t.ProjectPhase)                     
                    .ThenInclude(p => p.Project)           
                .Where(t => t.PhaseId == phaseId && t.ProjectPhase.ProjectId == projectId)
                .ToListAsync();
        }



        public async Task UpdateTaskAsync(ProjectPhaseTasks task)
        {
            if (task == null)
                throw new ArgumentException("La tarea no puede ser nula.");

            var existingTask = await _context.Tasks.FindAsync(task.TaskId);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.IsCompleted = task.IsCompleted;

                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<Project>> GetFilteredProjectsAsync(string searchText, string status)
        {
            var query = _context.Projects
                .Include(p => p.ProjectPhase)
                    .ThenInclude (p => p.ProjectPhaseTasks)
                .Include(p => p.Client)
                .Include(p => p.Admin)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => p.Name.Contains(searchText) || p.Description.Contains(searchText));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status == status);
            }

            return await query.ToListAsync();
        }

        public async Task UpdateProjectDetailsAsync(int id, string constructionType, string province, string canton, double constructionSize, double landSize, int bedrooms, int bathrooms, decimal price, int floors, bool hasGarage, int garageCapacity, bool isCondominium)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.ConstructionType = constructionType;
                project.Province = province;
                project.Canton = canton;
                project.ConstructionSize = constructionSize;
                project.LandSize = landSize;
                project.Bedrooms = bedrooms;
                project.Bathrooms = bathrooms;
                project.Price = price;
                project.Floors = floors;
                project.HasGarage = hasGarage;
                project.GarageCapacity = garageCapacity;
                project.IsCondominium = isCondominium;

                await _context.SaveChangesAsync();
            }
        }
    }
}