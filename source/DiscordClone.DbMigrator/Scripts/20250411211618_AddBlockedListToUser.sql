ALTER TABLE "AspNetUsers" ADD application_user_id uuid;

CREATE INDEX ix_asp_net_users_application_user_id ON "AspNetUsers" (application_user_id);

ALTER TABLE "AspNetUsers" ADD CONSTRAINT fk_asp_net_users_asp_net_users_application_user_id FOREIGN KEY (application_user_id) REFERENCES "AspNetUsers" (id);
