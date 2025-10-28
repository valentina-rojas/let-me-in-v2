-> introduccion

=== introduccion ===
¡Ojo con el techo, que el peinado es frágil! ¡Y caro!

+ [¿Hola...? ¿Hay alguien ahí?]
    -> saludo
+ [¿No lo veía... Y ese peinado?]
    -> peinado
+ [¿Qué trae para aportar al búnker?]
    -> aporte
-> END


=== saludo ===
Claro que sí. Soy Mauro. Estoy acá abajo. ¡Siga el jopo!

+ [¿Qué hace peinado así?]
     -> peinado
-> END


=== peinado ===
Es mi orgullo. ¿Sabía que me salvó de un murciélago volador una vez? Se enredó y no pudo salir.

+ [Vaya, impresionante...]
    Gracias, no todos los días un peinado salva vidas.
    -> END
+ [Deberíamos revisar su salud antes de dejarlo entrar.]
    No hace falta, estoy sano y estilizado.
    -> END
-> END


=== aporte ===
Altura moral y presencia estética. Si hay que buscar cosas debajo de estanterías o colarse por ductos, yo soy el hombre. Pequeño, pero útil.

+ [Solo puedo dejar entrar a quienes cumplan las normas de seguridad.]
    De todas maneras siempre encuentro la forma de entrar...
    -> END
-> END


=== sintomas ===
Nada. Este jopo no se cae ni con viento viral. Estoy sano, estilizado y con movilidad reducida en ascensores.

+ [Aún así necesito verificar su salud.]
   Esta bien, de todas maneras siempre encuentro la forma de entrar...
    -> END
-> END


=== respuestaIngreso ===
Gracias, como podés ver no voy a ocupar mucho espacio.
-> END

=== respuestaRechazo ===
No pasa nada, ya voy a encontrar algún ducto por el cual ingresar.
-> END
