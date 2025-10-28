-> introduccion
-> introduccion

=== introduccion ===
Hola, necesito entrar ya mismo.

+ [¿De dónde viene disfrazado así?]
    -> origenDisfraz
+ [¿Puede explicarme por qué los ajos y el colador?]
    -> motivoAjos
+ [Necesito ver si presenta algún síntoma, ¿puede quitarse los elementos?]
    -> quitarElementos
-> END


=== origenDisfraz ===
Si no te enteraste estamos en medio de una pandemia, estoy tomando precauciones.

+ [¿Qué precauciones exactamente?]
    -> precauciones
+ [Entiendo... pero debo verificar su estado de salud.]
    ¿Verificar? No tengo nada, estoy sano.
    -> END
-> END


=== precauciones ===
Me cubro con lo que puedo y uso el colador para filtrar las ondas.

+ [¿Las ondas?]
    -> motivoAjos
+ [¿Eso no es peligroso?]
    No, para nada. Me siento seguro así.
    -> END
-> END


=== motivoAjos ===
Para que el gobierno no me lave el cerebro con las ondas que emiten.

+ [¿En serio cree eso?]
    Sí, los ajos y el colador protegen.
    -> END
+ [Suena poco fiable; necesito pruebas de que está sano.]
    No voy a mostrarle nada, con esto es suficiente.
    -> END
-> END


=== quitarElementos ===
No voy a quitármelos. ¿Acaso quiere que me contagie? Seguro está trabajando para ellos...

+ [Si no coopera, no puedo dejarlo entrar.]
    Entonces buscaré otra entrada; yo no me voy a sacar esto.
    -> END
+ [Tranquilo, solo quiero asegurarme de que no representa un riesgo.]
    ¿Riesgo? Estoy sano, no me ponga en esa.
    -> END
-> END


=== respuestaIngreso ===
Gracias pero no me pienso sacar nada...
-> END

=== respuestaRechazo ===
¿Y ahora dónde voy a conseguir más ajos?
-> END
