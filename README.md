## Использование

eFarmaResultProcessor.exe _<исходный_файл.xml>_ _<файл_результатов.xml>_ _<файл_результатов.xml>_

Можно так же указать папку или несколько.
Самый простой способ - перетянуть папку или несколько .xml файлов прямо на .exe

## Конфигурация
|Параметр|Описание|
|-----|:-----|
|`OutputSuffix`|Суффикс для добавления к обработанному файлу. Если отсутствует или пустой, то заменяется исходный файл|
|`AutoCloseOnError`|Закрыть ли автоматически консоль приложения при наличии ошибок. Если `false`, то консоль не закрывается, позволяя прочитать текст ошибки. Если `true`, то консоль автоматически закроется при появлении ошибок (удобно для пакетной обработки). При отсутствии ошибок консоль закрывается в любом случае|
|`SourceNodes`|`XPath` путь к исходным документам в XML. Используется для поиска документов для удаления и определения типа .xml файла|
|`ResultNodes`|`XPath` путь к результатам в XML. Используется для поиска идентификаторов документов для удаления и определения типа .xml файла|
|`DocumentIdNode`|Имя тэга определяющего идентификатор документа|
|`StatusNode`|Имя тэга определяющего статус документа в результатах|
|`RemoveStatuses`|Список статусов для удаления из исходного документа|

## Возврат кодов ошибок _(для пакетной обработки)_
|Код|Описание|
|---|:----|
|0|Нет ошибок|
|1|Не указаны файлы/папки для обработки|
|2|Указанные файлы недоступны или не в том формате|
|3|Ошибка во время обработки|
|4|Необработанное исключение|