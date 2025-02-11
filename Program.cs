using System;
using System.IO;

class Program
{
    static void Main()
    {
        string origem = @"C:\Users\Adm\Dev\MeusProgramas";  // Pasta de origem
        string destino = @"X:\DEPARTAMENTO DE TI\Backup de codigos"; // Pasta de destino
        string arquivoLog = @"X:\DEPARTAMENTO DE TI\Backup de codigos\Backup_Log.txt"; // Documento que conterá as atualizações

        // Verifica se o caminho existe
        if (Directory.Exists(origem) == false)
        {
            Console.WriteLine("❌ A pasta de origem não existe.");
            return; // Encerra a execução
        }

        // Verifica se o caminho do destino existe
        if (Directory.Exists(destino) == false)
        {
            Directory.CreateDirectory(destino); // Cria o destino
        }

        // Escrever no arquivo log as modificações
        using (StreamWriter logStream = new StreamWriter(arquivoLog, true)) // True para sobrescrever
        {
            // Escreve no log o momento do backup
            logStream.WriteLine($"\nBackup realizado em: {DateTime.Now}");

            // Chama a função que copia os arquivos, passando a ela os parâmetros de origem, destino e o log
            CopiarDiretorio(origem, destino, logStream);
        }

        // Mensagem ao usuário
        Console.WriteLine("\n✅ Backup concluído!");
    }

    // Função que copiará os arquivos
    static void CopiarDiretorio(string origem, string destino, StreamWriter logStream)
    {
        // Percorrerá todos os arquivos, pastas e subpastas do diretório
        foreach (string dir in Directory.GetDirectories(origem, "*", SearchOption.AllDirectories))
        {
            // Irá passar à string criada 
            string destinoDir = dir.Replace(origem, destino);
            if (!Directory.Exists(destinoDir))
            {
                // Cria o diretório de destino caso não exista
                Directory.CreateDirectory(destinoDir);
                Console.WriteLine($"📁 Criado diretório: {destinoDir}");
                logStream.WriteLine($"📁 Criado diretório: {destinoDir}");
            }
        }

        // Copiar arquivos
        foreach (string arquivo in Directory.GetFiles(origem, "*", SearchOption.AllDirectories))
        {
            // Atribui o nome da origem ao nome do destino
            string destinoArquivo = arquivo.Replace(origem, destino);

            // Verifica se um arquivo já existe no destino para evitar sobrecargas desnecessárias
            if (File.Exists(destinoArquivo))
            {
                // Puxa a utima data do arquivo na origem e no destino
                DateTime origemMod = File.GetLastWriteTime(arquivo);
                DateTime destinoMod = File.GetLastWriteTime(destinoArquivo);

                if (origemMod > destinoMod) // Se o arquivo de origem for mais novo, sobrescreve
                {
                    File.Copy(arquivo, destinoArquivo, true); // True para adicionar ao que já existe
                    Console.WriteLine($"🔄 {Path.GetFileName(arquivo)} atualizado!");
                    logStream.WriteLine($"🔄 {Path.GetFileName(arquivo)} atualizado.");
                }
                else // Caso o arquivo de destino for mais novo
                {
                    Console.WriteLine($"⚡ {Path.GetFileName(arquivo)} já está atualizado.");
                    logStream.WriteLine($"⚡ {Path.GetFileName(arquivo)} já está atualizado.");
                }
            }
            else // Se não existir nenhum arquivo correspondente
            {
                // Cria um arquivo novo
                File.Copy(arquivo, destinoArquivo);
                Console.WriteLine($"✔ {Path.GetFileName(arquivo)} copiado!");
                logStream.WriteLine($"✔ {Path.GetFileName(arquivo)} copiado.");
            }
        }
    }
}