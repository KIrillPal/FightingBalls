# FightingBalls
Это 2D шутер для одного игрока. Это командный проект, написаны на C# на движке Unity 2021.1.1f1

Игрок представляет собой шарик, умеющий двигаться с помощью кнопок WASD и стрелять, нажимая ЛКМ. 
Он дерётся в одиночку с одним или несколькими противниками в зависимости от выбранного режима. Мы играем за красную каплю, стреляющую в противников (синих капель) мелкими шариками. При попадании в каплю, её размер уменьшается. Изначально капля может выдержать 3 попадания, а на четвёртом пропадёт, однако мы можем увеличивать свой размер с помощью зелёных квадратиков (далее "аптечек"), разбросанных по всему полю. Максимальный размер капель всех типов выдерживает 5 попаданий. Цель игры - уничтожить всех противников. 

Сразу после выбора режима мы появляемся в центе одной из трёх карт для поединка. Если противник один, то он появляется в правом верхнем углу поля. Все противники вне зависимости от типа не стремяться затягивать бой. Ждите, что они будут атаковать. Противник, так же как и игрок, умеет стрелять с разной точностью и скоростью, уворачиваться от пуль, отступать и восстанавливать свой рамзер аптечками. Умные боты имеют сложный алгоритм передвижения, рассчианный на запутывание игрока.

При нажатии ЛКМ происходит выстрел по направлению к курсору мыши. Игрок имеет абсолютную точность стрельбы, т.е. пуля всегда летит по прямой к курсору. Количество выпускаемых пуль в секунду не ограничено. При каждом нажатии ЛКМ будет вылетать пуля. Попадая в аптечку или другую пулю, она уничтожается вместе с этим объектом. Таким образом, стреляя в пули противников, можно защищаться от потери "жизней" - т.е. остатка размера, который есть у капли.

Также имеется 2 типа оружия:
1. В боях 1 на 1 используется простое оружие. Оно стреляет одиночными выстрелами при нажатии ЛКМ.
2. В режиме "Посмотреть всех" используется пулемёт. Он стреляет бесконечно при зажатом ЛКМ. Не забывайте, что одиночные нажатия с ним неэффективны.

Теперь разберёмся в нюансах игрового процесса.
## 1. Боты
Для сражений игроку представлено 3 типа ботов. 

1. Кретин (называл не я). Это лёгкий уровень бота, он олицетворяет собой неопытного игрока. У него понижена реакция на летящие в него пули, он не пытается обходить, т.к. не заинтересован отступать далеко. Стреляет быстро со скоростью семь пуль в секунду. Примерно такая скорость клика у обычного игрока. Кретин имеет среднюю точность стрельбы, также он не пытается окружить, не стреляет на упреждение. Итогом этого становится его лёгкая уязвимость. Однако он имеет неплохой огневой потенциал, и если подпустить его достаточно близко, то максимальное время жизни составит менее секунды. Он идентичен с игроком по скорости и живучести (минимальному количеству попаданий для уничтожения) и всем остальным параметрам среди тех, которые не относятся к умениям пользователя.

2. Норм. Это самый интересный тип бота для сражения 1 на 1. Он имеет хорошиу реакцию (5 мс), умеет уворачиваться от пуль, стрелять на упреждение, обходить и выходить с неожиданных для игрока позиций (бот писался с учётом того, что его не будет видно за стенами). 

Все игровые характеристики Норма идентичны характеристикам игрока, но его алгоритм делает его сложным для уничтожения. Его тактика зависит от близости к игроку и количеству жизней у него и игрока. В близи он начинает "кружить" игрока, т.е. вертеться вокруг нас в зависимости от близости к нему выпущенных нами пуль. Оказавшись вне поля зрения (ожидается, что это просто будет на большом расстоянии), он умеет обходить и выходить с другой стороны. Чем больше его количество жизней, тем более смело он действует. Он начинает сильнее сближаться с игроком, меньше стреляет по пулям игрока (для защиты), а больше по нам. При такой тактике он не даёт нам возможности безнаказанно отступить от него, а также умеет эффективно играть как вплотную, так и на расстоянии, с большим количеством жизней или без.

3. Усатюк. Имя было выбрано в честь одного хорошего препода Физтех-Лицея. Оно означает, что этого бота невозможно победить с обычным оружием. Точнее при тестировании это удалось один раз, но его характеристики слишком сильны для обычного игрока. Бот является практически неуязвимым не из-за алгоритма, а из-за высоких игровых характеристик. 

При скорости его пуль на 20% выше, чем у игрока и остальных ботов, он стреляет 20 раз в секунду. Из-за ужасной точности его пулемёта, вокруг него всегда образуется "облако" из пуль, что сильно усложняет возможность его ранить. Однако на третьей волне режима "Посмотреть всех" с ним можно сразиться практически на равных. Наш пулемёт имеет меньшую скорость полёта пуль, однако идеальную точность. Сложно сказать, какой из пулемётов лучше. В любом случае, он является боссом режима.

# Подключение и использование
Чтобы запустить приложение, скачайте архив [Release_visible.zip](https://github.com/KIrillPal/FightingBalls/blob/main/Release_visible.zip) из корня проекта и запустите в нём файл FightingBalls.exe. Он слегка отличается от изначальной версии игры, представленной на Github, тем, что игрок может видеть противников, находящихся за стенами. Это добавляет равноправия в игру. Изначальная версия проекта: [Release.zip](https://github.com/KIrillPal/FightingBalls/blob/main/Release.zip).

## Режим с волнами

Для интересного тестирования ботов был создан режим "Посмотреть всех". Он разбит на 3 волны.
1. Уничтожить 3 ботов "Кретин"
1. Уничтожить 2 ботов "Норм"
1. Уничтожить 1 бота "Усатюк"

В этом режиме доступно "фановое" оружие - пулемёт, выпускающий 20 пуль в секуднду. Чтобы его использовать, нужно зажать ЛКМ. Такая скорострельность позволяет пробивать "щиты из пуль", выпускаемых любым противником.

# Структура проекта
Весь код игры, написанный вручную, содержится в папке [Scripts](https://github.com/KIrillPal/FightingBalls/blob/main/Assets/Scripts). Опишем имеющиеся здесь файлы.

## 1. GameController.cs
Этот файл отвечает за работу меню.

## 2. UIController.cs
Этот файл отвечает за смену отображаемых экранов (экрана меню, смерти, победы).
## 3. EnemyAmountController.cs
Этот файл отвечает за корректный запуск и завершение уровней и волн.
## 4. CameraMovement.cs
Здесь положение камеры привязывается к точке чуть сзади игрока (это сделано для увеличения поля зрения спереди от игрока).
## 5. BoneMovement.cs
Отвечает за обновление точек, к которым двигаются боты. 
## 6. FieldOfViewScript.cs
Этот файл создаёт область тени за стенами. Обычно эта область слепая и в ней не видно противников, но, как говорилось выше, есть версия игры, где это просто тень.
## 7. Shooting.cs
Он создаёт и отправляет пули в нужном направлении.
## 8. BulletScript.cs
Он отвечает за обработку соприкосновений пуль с объектами.
## 9. HealerControllScript.cs (Так его назвал Степан)
Обрабатывает появление аптечек на поле.
## 10. HealPointScript.cs
Обрабатывает использование аптечек.
## 11. LayoutControllerScript.cs
Отключает все слои перед началом боя.
## 12. EnemyController.cs
Самый большой скрипт игры. Он отвечает за всё поведение ботов. Элементы алгоритма, из которых складываются итоговые реакции бота, подписаны комментариями.

# Замечания
Стоит отметить несколько достоинств игры и недостатков. 

Главное достоинство игры в равноправии ботов и игрока. По своим игровым характеристикам боты "Кретин" и "Норм" полностью соответствуют нам. Единственное их отличие в алгоритмах работы. Конечно, есть точность, скорость стрельбы и реакции. Их нельзя настроить игроку, однако они выбраны на основе тестов и на глаз исходя из возможностей игравших одноклассников в эту игру. Ещё хочется похвастаться уровнем игры бота "Норм", который не утупает по уровню игры человеку. Также в игре имеются неплохая музыка и несложный, но стильный дизайн. Много времени было потрачено на модели "Мягких шариков" у игрока и ботов. 

Главный недостаток игры в том, что музыка не отключается. Также из-за модели "Мягких шаров" иногда несколько ботов могут слипаться друг с другом. Это снижает их эффективность. Иногда возле стен плохо работает алгоритм поиска пути, что приводит к их застреванию. Наконец, идея с генетическим алгоритмом для ботов могла быть осуществлена, но была отброшена из-за нехватки времени на работу.

# Особенности проекта
Это командный проект. Он сделан двумя учащимися 11 класса: [stepan2409](https://github.com/stepan2409) и [KIrillPal](https://github.com/KIrillPal). Первый заложил фундамент проекта - создал все скрипты, карты, их загрузку. Создал игрока и пустых ботов (без ИИ). Написал работу волн, реализацию стрельбы, поля зрения, подключил для поиска пути алгоритм A\*. Второй написал ИИ для ботов, улучшил работу пуль, добавил пулемёт, настроил работу аптечек для ботов, сделал каплям мягкую модель.
