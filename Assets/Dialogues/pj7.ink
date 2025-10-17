-> introduccion

=== introduccion ===
Hola, escuché que en este lugar hay mucha gente.
+ [¿De dónde viene?]
    -> procedencia
+ [¿Estuvo en contacto con otra gente?]
    -> contacto

=== procedencia ===
En la plaza, no hay nadie para jugar un partido, como siempre.
+ [¿Presentó síntomas como picazón o sarpullido?]
    -> sintomas
+ [¿Cómo puede estar tan en forma si nadie sale?]
    -> ejercicio

=== contacto ===
No, si todos están encerrados, parece que soy el único que quiere moverse.
+ [¿Presentó síntomas como picazón o sarpullido?]
    -> sintomas

=== sintomas ===
Ninguno, mi estado físico es excelente, no como todos los que se escondieron en el sillón de su casa.
+ [¿Cómo mantuvo su estado físico si no se podía salir?]
    -> ejercicio
+ [¿Está seguro de no haber estado con alguien?]
    Claro, no necesito compañía para sudar.
    -> END

=== ejercicio ===
Ejercitando en casa, no es lo mismo que un buen partido, pero al menos me mantengo en forma.
+ [¿Cómo puedo estar seguro de que no se encontró con otra gente para jugar?]
    ¿No ve que no tengo ningún síntoma? Me saco la remera de ser necesario, pero déjeme entrar por favor, ¡necesito jugar un partido!
    -> END
    

=== respuestaIngreso ===
Gracias loco, los voy a sacar atletas a todos...
-> END

=== respuestaRechazo ===
Bueno, voy a ver si alguien de la fila quiere jugar un rato...
-> END


