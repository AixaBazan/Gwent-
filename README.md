Segundo Proyecto de Programación
Gwent ++

Gwent-CardGame es un juego de cartas, presentando características comunes a la mayoría de juegos pertenecientes a este amplio género:

    Se enfrentan dos jugadores en cada partida.
    Cada jugador posee una facción y una imagen identificación, puntos obtenidos, y un conjunto de cartas, las cuales pueden encontrarse en la mano, el campo, el cementerio o el mazo.
    Cada carta posee un nombre y una imagen que la identifican, un texto de descripción, puntos de ataque, un tipo y un conjunto de efectos opcionales.
    Cada efecto tiene una fase de activación, unas condiciones de activación y unas acciones.
    La mano de cada jugador tiene un conjunto de hasta 10 cartas. Cualquier carta que sea añadida a la mano si esta ya está llena es automáticamente es enviada al cementerio.
    El mazo es el conjunto inicial de cartas de cada jugador el cual tiene mínimo 25 cartas.
    Gana el jugador que más puntos presenta al final de la partida.

Creación de nuevas cartas:
    El proyecto permite que el usuario antes de comenzar una partida cree sus propias cartas y efectos. Para ello fue ingeniado un Lenguaje de Propósito Específico (DSL). En dicho lenguaje: 
        • Se aceptan operadores aritméticos (+, -, *, /, ^, ++, --), lógicos (&&, ||), de comparación 
        (<, >, >=, <=), concatenación de cadenas (@, @@) o de asignación (=).

        El operador @@ incluye un espacio entre las cadenas que se concatenan.

        • Se acepta declaraciones de constantes y variable, (number = 5)

        • Se aceptan accesos a propiedades tanto del contexto (context.Hand) como de una carta (card.Power)

        • Se acepta el indexado en listas (context.Hand.Find((card) => true)[0])

        • Se aceptan ciclos en listas (for y while) (for target in targets, while (i < count)).

        Las funciones y el cuerpo de los ciclos pueden ser tanto una expresión como un bloque de expresiones. Un bloque de expresiones es declarar varias expresiones entre llaves y terminadas en ; (como se puede apreciar en los ejemplos de Action de los efectos). Una sola expresión se ve en el ejemplo del Predicate del selector en la carta.

    Este lenguaje es procesado y traducido a objetos interactivos en el juego. Este proceso es llevado a cabo en varias etapas. A continuación, se describen cada una de ellas:

   Análisis léxico :
        El análisis léxico, también conocido como lexer, es primera etapa en el proceso de compilación de un lenguaje de programación. Su función principal es transformar una secuencia de caracteres de código fuente en una secuencia de tokens, los cuales representan unidades léxicas significativas.
        Si el lexer encuentra un carácter o una secuencia de caracteres que no coincidan con definición ninguna léxica válida, se genera un error, si la lista de errores luego de este análisis es vacía, se pasa a la siguiente etapa, sino son informados al usuario para que pueda corregirlos y continuar la evaluación. 

   Análisis sintáctico :
        El parser maneja el análisis sintáctico o gramatical del código y su conversión al árbol de sintaxis abstracta correspondiente, mediante los tokens devueltos por el lexer.
        Si en este proceso se encuentra una secuencia de tokens invalida (ya sea porque no sigan un orden o una estructura correcta), se genera error. Los errores se manejan como mismo en la etapa anterior.

    Análisis Semántico:
        Al superar la etapa del parser, ya obtenemos nuestro AST. Lo siguiente es analizar que cada nodo este correcto semánticamente. Para ello contamos con la clase abstracta AST, la cual presenta un método CheckSemantic. Como cada nodo del árbol hereda de dicha clase, cada uno se va a analizar semánticamente y va a llamar recursivamente al método CheckSemantic en los nodos hijos para evaluar la semántica de las subexpresiones. Algunas de las comprobaciones que se realizan son las siguientes:
            Verificación de tipos. Asegurar que los operandos de un operador sean del tipo adecuado (por ejemplo, no sumar un número con una cadena).
            Verificar que todas las variables utilizadas en el programa han sido declaradas previamente.
            Asegurar que las variables se utilicen dentro de su ámbito de declaración.
            Verificar que a los métodos se les pase un parámetro valido
		Verificar que se acceda correctamente a las propiedades y métodos. 
            Entre otras.
	Los errores se manejan de la misma forma, si algún nodo presenta una semántica invalida se genera un error el cual es guardado en la lista de errores para ser informados posteriormente al usuario.

    Evaluación:
        Si se atravesaron satisfactoriamente las etapas anteriores, se pasa a la última etapa: Evaluación o Interpretación. En esta cada nodo se evalúa y finaliza con la construcción de las cartas las cuales serán añadidas al deck del jugador según la facción especificada.

Conclusiones:
Con esta actualización se les permite a los jugadores crear cartas personalizadas, lo que enriquece la experiencia de juego fomentado la creatividad. 

