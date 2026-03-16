using Microsoft.Playwright;

namespace FastSpam;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("输入要发送的短评文字：");
        String spamText = Console.ReadLine();
        Console.WriteLine("输入目标用户主页id:");
        string id = Console.ReadLine();
        Console.WriteLine("输入要刷几层楼");
        int times = int.Parse(Console.ReadLine());

        var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
       
        var context = playwright.Chromium
            .LaunchPersistentContextAsync("user_data",
                new BrowserTypeLaunchPersistentContextOptions { Headless = false }).GetAwaiter().GetResult();

        var page = context.Pages.Count > 0 ? context.Pages[0] : context.NewPageAsync().GetAwaiter().GetResult();
        
            Console.WriteLine("请在弹出的浏览器中手动登录。如果已登录，回车继续");
            page.GotoAsync("https://www.mcmod.cn/login/").GetAwaiter().GetResult();
            Console.WriteLine("登录成功后，请按回车键继续...");
            Console.ReadLine();
        
        page.GotoAsync("https://center.mcmod.cn/" + id).GetAwaiter().GetResult();
        Thread.Sleep(2000);
        page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)").GetAwaiter().GetResult();
        for (int i = 0; i < times; i++)
        {
            System.Threading.Thread.Sleep(2000);

            page.EvaluateAsync("text => editor.setContent(text)", spamText).GetAwaiter().GetResult();
            System.Threading.Thread.Sleep(1000);

            page.ClickAsync("#comment-submit").GetAwaiter().GetResult();
            System.Threading.Thread.Sleep(3000);

            page.Locator(".comment-delete").First.ClickAsync().GetAwaiter().GetResult();
            System.Threading.Thread.Sleep(1000);
            page.Locator(".swal2-confirm").ClickAsync().GetAwaiter().GetResult();
            System.Threading.Thread.Sleep(2000);
        }
    }
}