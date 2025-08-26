using BabsKitapEvi.Entities.Models;
using BabsKitapEvi.Entities.Static;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BabsKitapEvi.DataAccess
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            await context.Database.MigrateAsync();

            await SeedRolesAsync(roleManager);
            await SeedAdminUserAsync(userManager);
            await SeedCategoriesAndBooksAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new AppRole { Name = Roles.Admin });
            }
            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new AppRole { Name = Roles.User });
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin);
                }
            }
        }

        private static async Task SeedCategoriesAndBooksAsync(ApplicationDbContext context)
        {
            if (context.Books.Any()) return;

            context.BookCategories.RemoveRange(context.BookCategories);
            context.Books.RemoveRange(context.Books);
            context.Categories.RemoveRange(context.Categories);
            await context.SaveChangesAsync();

            var categories = new Dictionary<string, Category>
            {
                {"Roman", new Category { Name = "Roman" }},
                {"Polisiye", new Category { Name = "Polisiye" }},
                {"Gizem", new Category { Name = "Gizem" }},
                {"Fantastik", new Category { Name = "Fantastik" }},
                {"Tarih", new Category { Name = "Tarih" }},
                {"Gerilim", new Category { Name = "Gerilim" }},
                {"Aşk", new Category { Name = "Aşk" }},
                {"Psikolojik", new Category { Name = "Psikolojik" }},
                {"Modern Klasik", new Category { Name = "Modern Klasik" }},
                {"Klasik", new Category { Name = "Klasik" }},
                {"Distopya", new Category { Name = "Distopya" }},
                {"Bilimkurgu", new Category { Name = "Bilimkurgu" }},
                {"Biyografi", new Category { Name = "Biyografi" }},
                {"Genç Yetişkin", new Category { Name = "Genç Yetişkin" }}
            };

            await context.Categories.AddRangeAsync(categories.Values);
            await context.SaveChangesAsync();

            var books = new List<Book>
            {
                new Book { Title = "Suç ve Ceza", Author = "Fyodor Dostoyevski", ISBN = "978-605-332-520-2", PublishedDate = new DateTime(1866, 1, 1), Description = "Raskolnikov'un ahlaki çatışmaları.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000663939-1.jpg", StockQuantity = 55, Price = 135.50m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Klasik"] }, new BookCategory { Category = categories["Psikolojik"] } } },
                new Book { Title = "Sefiller", Author = "Victor Hugo", ISBN = "978-605-332-603-2", PublishedDate = new DateTime(1862, 1, 1), Description = "19. yüzyıl Fransa'sında adalet ve insanlık.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000663940-1.jpg", StockQuantity = 40, Price = 155.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Klasik"] }, new BookCategory { Category = categories["Tarih"] } } },
                new Book { Title = "İnce Memed", Author = "Yaşar Kemal", ISBN = "978-975-08-0711-0", PublishedDate = new DateTime(1955, 1, 1), Description = "Çukurova'da ağalık düzenine başkaldırı.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000106333-1.jpg", StockQuantity = 80, Price = 148.50m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Roman"] }, new BookCategory { Category = categories["Modern Klasik"] } } },
                new Book { Title = "Kürk Mantolu Madonna", Author = "Sabahattin Ali", ISBN = "978-975-363-801-1", PublishedDate = new DateTime(1943, 1, 1), Description = "Raif Efendi'nin Berlin'deki aşkı.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000064242-1.jpg", StockQuantity = 90, Price = 82.50m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Aşk"] }, new BookCategory { Category = categories["Modern Klasik"] } } },
                new Book { Title = "Saatleri Ayarlama Enstitüsü", Author = "Ahmet Hamdi Tanpınar", ISBN = "978-975-99-5598-4", PublishedDate = new DateTime(1961, 1, 1), Description = "Türk toplumunun modernleşme sancıları.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000105398-1.jpg", StockQuantity = 60, Price = 120.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Roman"] }, new BookCategory { Category = categories["Modern Klasik"] } } },
                new Book { Title = "1984", Author = "George Orwell", ISBN = "978-975-07-1852-9", PublishedDate = new DateTime(1949, 6, 8), Description = "Totaliter rejim ve 'Büyük Birader'.", ImageUrl = "https://res.cloudinary.com/dp7utrng4/image/upload/v1755773242/babs-kitap-evi/books/GeorgeOrwell-1984_vshybq.jpg", StockQuantity = 75, Price = 98.75m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Distopya"] }, new BookCategory { Category = categories["Klasik"] } } },
                new Book { Title = "Cesur Yeni Dünya", Author = "Aldous Huxley", ISBN = "978-975-69-2918-4", PublishedDate = new DateTime(1932, 1, 1), Description = "Teknolojinin toplumu kontrol ettiği bir dünya.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000105399-1.jpg", StockQuantity = 65, Price = 110.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Distopya"] }, new BookCategory { Category = categories["Bilimkurgu"] } } },
                new Book { Title = "Dune", Author = "Frank Herbert", ISBN = "978-605-77-6239-1", PublishedDate = new DateTime(1965, 8, 1), Description = "Arrakis gezegeninde geçen epik bir bilimkurgu.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0001886893001-1.jpg", StockQuantity = 95, Price = 250.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Bilimkurgu"] }, new BookCategory { Category = categories["Fantastik"] } } },
                new Book { Title = "Ben, Robot", Author = "Isaac Asimov", ISBN = "978-605-37-5782-6", PublishedDate = new DateTime(1950, 1, 1), Description = "Üç Robot Yasası ve robot psikolojisi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000105400-1.jpg", StockQuantity = 70, Price = 145.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Bilimkurgu"] }, new BookCategory { Category = categories["Klasik"] } } },
                new Book { Title = "Yüzüklerin Efendisi: Yüzük Kardeşliği", Author = "J.R.R. Tolkien", ISBN = "978-975-34-2347-7", PublishedDate = new DateTime(1954, 7, 29), Description = "Orta Dünya'da geçen epik bir macera.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000063363-1.jpg", StockQuantity = 110, Price = 280.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Fantastik"] } } },
                new Book { Title = "Harry Potter ve Felsefe Taşı", Author = "J.K. Rowling", ISBN = "978-975-08-0294-8", PublishedDate = new DateTime(1997, 6, 26), Description = "Genç bir büyücünün Hogwarts'taki maceraları.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000106331-1.jpg", StockQuantity = 150, Price = 125.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Fantastik"] }, new BookCategory { Category = categories["Genç Yetişkin"] } } },
                new Book { Title = "Kralların Çarpışması", Author = "George R.R. Martin", ISBN = "978-605-47-2901-2", PublishedDate = new DateTime(1998, 11, 16), Description = "Westeros'ta taht kavgaları devam ediyor.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000441292-1.jpg", StockQuantity = 85, Price = 320.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Fantastik"] }, new BookCategory { Category = categories["Roman"] } } },
                new Book { Title = "Doğu Ekspresinde Cinayet", Author = "Agatha Christie", ISBN = "978-975-21-0912-3", PublishedDate = new DateTime(1934, 1, 1), Description = "Hercule Poirot'nun en karmaşık vakalarından biri.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000105388-1.jpg", StockQuantity = 130, Price = 95.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Polisiye"] }, new BookCategory { Category = categories["Gizem"] } } },
                new Book { Title = "Ejderha Dövmeli Kız", Author = "Stieg Larsson", ISBN = "978-605-47-0201-5", PublishedDate = new DateTime(2005, 8, 1), Description = "Gazeteci Mikael Blomkvist ve hacker Lisbeth Salander'in gizemli bir olayı araştırması.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000363108-1.jpg", StockQuantity = 70, Price = 185.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Polisiye"] }, new BookCategory { Category = categories["Gerilim"] } } },
                new Book { Title = "Kuzuların Sessizliği", Author = "Thomas Harris", ISBN = "978-975-21-0913-0", PublishedDate = new DateTime(1988, 5, 29), Description = "FBI ajanı Clarice Starling ve Dr. Hannibal Lecter'ın gerilim dolu ilişkisi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000105389-1.jpg", StockQuantity = 65, Price = 160.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Gerilim"] }, new BookCategory { Category = categories["Psikolojik"] } } },
                new Book { Title = "Da Vinci Şifresi", Author = "Dan Brown", ISBN = "978-975-21-0914-7", PublishedDate = new DateTime(2003, 3, 18), Description = "Robert Langdon'un sanat tarihi ve gizem dolu macerası.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000105390-1.jpg", StockQuantity = 100, Price = 175.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Gizem"] }, new BookCategory { Category = categories["Gerilim"] } } },
                new Book { Title = "Ve Sonra Hiç Kalmadı", Author = "Agatha Christie", ISBN = "978-975-21-0915-4", PublishedDate = new DateTime(1939, 11, 6), Description = "On yabancının ıssız bir adadaki gizemli ölümleri.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000105391-1.jpg", StockQuantity = 140, Price = 105.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Polisiye"] }, new BookCategory { Category = categories["Gizem"] } } },
                new Book { Title = "Sapiens: İnsan Türünün Kısa Bir Tarihi", Author = "Yuval Noah Harari", ISBN = "978-605-98-5204-9", PublishedDate = new DateTime(2011, 1, 1), Description = "İnsanlığın evrimsel ve tarihsel yolculuğu.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000629431-1.jpg", StockQuantity = 180, Price = 220.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Tarih"] } } },
                new Book { Title = "Nutuk", Author = "Mustafa Kemal Atatürk", ISBN = "978-975-49-4001-8", PublishedDate = new DateTime(1927, 10, 1), Description = "Türkiye Cumhuriyeti'nin kuruluş mücadelesi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000063364-1.jpg", StockQuantity = 250, Price = 75.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Tarih"] }, new BookCategory { Category = categories["Biyografi"] } } },
                new Book { Title = "Steve Jobs", Author = "Walter Isaacson", ISBN = "978-605-09-0800-0", PublishedDate = new DateTime(2011, 10, 24), Description = "Apple'ın kurucusunun hayat hikayesi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000374999-1.jpg", StockQuantity = 90, Price = 290.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Biyografi"] } } },
                new Book { Title = "Bir Ömür Nasıl Yaşanır?", Author = "İlber Ortaylı", ISBN = "978-605-76-8301-6", PublishedDate = new DateTime(2019, 1, 1), Description = "Tarihçi İlber Ortaylı'dan hayat tavsiyeleri.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0001797303001-1.jpg", StockQuantity = 200, Price = 115.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Biyografi"] } } },
                new Book { Title = "Açlık Oyunları", Author = "Suzanne Collins", ISBN = "978-605-44-8201-8", PublishedDate = new DateTime(2008, 9, 14), Description = "Panem'in distopik dünyasında hayatta kalma mücadelesi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000306906-1.jpg", StockQuantity = 160, Price = 140.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Genç Yetişkin"] }, new BookCategory { Category = categories["Distopya"] } } },
                new Book { Title = "Kağıttan Kentler", Author = "John Green", ISBN = "978-605-53-4047-1", PublishedDate = new DateTime(2008, 10, 16), Description = "Quentin'in gizemli komşusu Margo'yu arayışı.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000585399-1.jpg", StockQuantity = 110, Price = 130.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Genç Yetişkin"] }, new BookCategory { Category = categories["Gizem"] } } },
                new Book { Title = "Yüzyıllık Yalnızlık", Author = "Gabriel García Márquez", ISBN = "978-975-07-2123-9", PublishedDate = new DateTime(1967, 5, 30), Description = "Buendía ailesinin birçok kuşağını anlatan roman.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000110936-1.jpg", StockQuantity = 30, Price = 142.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Roman"] }, new BookCategory { Category = categories["Klasik"] } } },
                new Book { Title = "Simyacı", Author = "Paulo Coelho", ISBN = "978-975-07-2697-5", PublishedDate = new DateTime(1988, 1, 1), Description = "Endülüslü çoban Santiago'nun masalsı yaşamı.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000064558-1.jpg", StockQuantity = 120, Price = 95.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Roman"] } } },
                new Book { Title = "Dönüşüm", Author = "Franz Kafka", ISBN = "978-605-332-030-6", PublishedDate = new DateTime(1915, 1, 1), Description = "Gregor Samsa'nın bir sabah dev bir böceğe dönüşmesi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000588831-1.jpg", StockQuantity = 65, Price = 78.90m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Modern Klasik"] } } },
                new Book { Title = "Otostopçunun Galaksi Rehberi", Author = "Douglas Adams", ISBN = "978-605-17-1013-4", PublishedDate = new DateTime(1979, 10, 12), Description = "Arthur Dent'in galaktik maceraları.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000681333-1.jpg", StockQuantity = 88, Price = 155.50m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Bilimkurgu"] } } },
                new Book { Title = "Fahrenheit 451", Author = "Ray Bradbury", ISBN = "978-605-37-5252-4", PublishedDate = new DateTime(1953, 10, 19), Description = "Kitapların yasaklandığı bir gelecekte itfaiyecilerin görevi kitap yakmaktır.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000508396-1.jpg", StockQuantity = 77, Price = 112.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Distopya"] }, new BookCategory { Category = categories["Bilimkurgu"] } } },
                new Book { Title = "Uçurtma Avcısı", Author = "Khaled Hosseini", ISBN = "978-605-14-1911-3", PublishedDate = new DateTime(2003, 5, 29), Description = "Afganistan'da geçen dokunaklı bir dostluk hikayesi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000558890-1.jpg", StockQuantity = 115, Price = 165.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Roman"] }, new BookCategory { Category = categories["Tarih"] } } },
                new Book { Title = "Hayvan Çiftliği", Author = "George Orwell", ISBN = "978-975-07-2038-6", PublishedDate = new DateTime(1945, 8, 17), Description = "Bir çiftlikteki hayvanların totaliter rejime başkaldırısı.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000404464-1.jpg", StockQuantity = 95, Price = 65.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Klasik"] }, new BookCategory { Category = categories["Distopya"] } } },
                new Book { Title = "Satranç", Author = "Stefan Zweig", ISBN = "978-605-33-2803-6", PublishedDate = new DateTime(1942, 1, 1), Description = "Bir gemi yolculuğunda geçen psikolojik bir gerilim.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000588832-1.jpg", StockQuantity = 180, Price = 45.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Psikolojik"] }, new BookCategory { Category = categories["Modern Klasik"] } } },
                new Book { Title = "Beyaz Zambaklar Ülkesinde", Author = "Grigory Petrov", ISBN = "978-605-33-2804-3", PublishedDate = new DateTime(1923, 1, 1), Description = "Finlandiya'nın yeniden doğuş hikayesi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000588833-1.jpg", StockQuantity = 130, Price = 55.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Tarih"] } } },
                new Book { Title = "Martin Eden", Author = "Jack London", ISBN = "978-605-33-2805-0", PublishedDate = new DateTime(1909, 1, 1), Description = "Genç bir denizcinin yazar olma mücadelesi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000588834-1.jpg", StockQuantity = 80, Price = 125.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Klasik"] }, new BookCategory { Category = categories["Roman"] } } },
                new Book { Title = "Babaya Mektup", Author = "Franz Kafka", ISBN = "978-605-33-2806-7", PublishedDate = new DateTime(1919, 1, 1), Description = "Kafka'nın babasıyla olan karmaşık ilişkisi.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000588835-1.jpg", StockQuantity = 70, Price = 60.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Biyografi"] }, new BookCategory { Category = categories["Psikolojik"] } } },
                new Book { Title = "İnsan Ne İle Yaşar?", Author = "Lev Tolstoy", ISBN = "978-605-33-2807-4", PublishedDate = new DateTime(1885, 1, 1), Description = "İnsan doğası ve ahlak üzerine kısa öyküler.", ImageUrl = "https://i.dr.com.tr/cache/600x600-0/originals/0000000588836-1.jpg", StockQuantity = 200, Price = 40.00m, BookCategories = new List<BookCategory> { new BookCategory { Category = categories["Klasik"] } } }
            };

            var existingIsbns = new HashSet<string>();
            var booksToAdd = new List<Book>();

            foreach (var book in books)
            {
                var uniqueIsbn = book.ISBN;
                while (existingIsbns.Contains(uniqueIsbn))
                {
                    uniqueIsbn = $"978-0-00-{(new Random().Next(100000, 999999))}-{booksToAdd.Count}";
                }

                book.ISBN = uniqueIsbn;
                booksToAdd.Add(book);
                existingIsbns.Add(uniqueIsbn);
            }

            await context.Books.AddRangeAsync(booksToAdd);
            await context.SaveChangesAsync();
        }
    }
}