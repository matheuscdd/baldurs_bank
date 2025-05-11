import { z } from 'zod';

export const userSchemaLogin = z.object({
    email: z.string().trim().email(),
    password: z.string().trim()
});

export type tUserLogin = z.infer<typeof userSchemaLogin>;
