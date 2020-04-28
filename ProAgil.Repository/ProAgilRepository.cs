using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using ProAgil.Domain;
using Microsoft.EntityFrameworkCore;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;

            //Qto ao AsNoTracking abaixo está uma forma de configuração geral para as query
            //É o mesmo que dizer que qdo o ambiente for de query não quero que seja rastreavel
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        //GERAIS
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            //Se salvar algum registo - no caso maior que zero retorna verdadeiro
            //Caso contrario - Falso
            return (await _context.SaveChangesAsync() > 0);
        }

        //EVENTOS
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(L => L.Lotes)
            .Include(RS => RS.RedeSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(pe => pe.PalestranteEventos)
                .ThenInclude(p => p.Palestrante);
            }

            //.AsNoTracking() -- É o mesmo que dizer não vá ao 
            //meu recurso para que ela nao seja retornado
            query = query.AsNoTracking()
            .OrderByDescending(dr => dr.DataEvento);

            return await query.ToArrayAsync();

        }


        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
           .Include(L => L.Lotes)
           .Include(RS => RS.RedeSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(pe => pe.PalestranteEventos)
                .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
            .OrderByDescending(dr => dr.DataEvento)
            .Where(T => T.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(L => L.Lotes)
            .Include(RS => RS.RedeSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(pe => pe.PalestranteEventos)
                .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
            .OrderByDescending(dr => dr.DataEvento)
            .Where(t => t.Id == EventoId);

            return await query.FirstOrDefaultAsync();
        }


        //PALESTRANTES
        public async Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(RS => RS.RedeSociais);

            if (includeEventos)
            {
                query = query
                .Include(pe => pe.PalestranteEventos)
                .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Nome)
            .Where(t => t.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestranteAsyncByName(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(RS => RS.RedeSociais);

            if (includeEventos)
            {
                query = query
                .Include(pe => pe.PalestranteEventos)
                .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Nome)
            .Where(t => t.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

    }
}