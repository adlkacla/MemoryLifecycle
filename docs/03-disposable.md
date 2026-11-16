# Запуск:

```bash
4. LogFile: корректный и некорректный сценарии
[LogFile 1] файл открыт: C:\Users\n3k\AppData\Local\Temp\memory-lifecycle.log
[LogFile 1] файл закрыт
После корректного using-блока
[LogFile 2] файл открыт: C:\Users\n3k\AppData\Local\Temp\memory-lifecycle.log
Метод с забытым Dispose завершён
[LogFile 2] финализатор! — файл не был закрыт явно
После сборки: сработал финализатор
```
