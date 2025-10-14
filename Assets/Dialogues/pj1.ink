-> introduccion

=== introduccion ===
Hola... llevo horas esperando.
+ [¿Estuvo en contacto con otras personas?]
    -> contactoPersonas
+ [¿Por qué está cubierta de tierra?]
    -> lluviaCamino

=== contactoPersonas ===
No, no me crucé con nadie en el camino.
+ [¿Seguro que nadie más estuvo cerca?]
    Sí, estoy segura, no vi a nadie.
    -> END
+ [¿Se siente bien de salud?]
    Sí, solo estoy cansada y hambrienta.
    -> END

=== lluviaCamino ===
Me agarró la lluvia en el camino y me perdí.
+ [¿Cómo puedo saber que no me está mintiendo?]
    Mire mi piel, no tengo ningún síntoma solo necesito una ducha.
    -> END
+ [No puedo realizar un diagnóstico por el estado en que se encuentra.]
    Mire mi piel, no tengo ningún síntoma solo necesito una ducha.
    -> END

=== respuestaIngreso ===
¡Gracias! Por fin voy a poder sacarme esta tierra.
-> END

=== respuestaRechazo ===
Entiendo... voy a seguir caminando...
-> END
