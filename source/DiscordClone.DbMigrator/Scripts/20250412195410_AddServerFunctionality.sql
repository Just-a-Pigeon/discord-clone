ALTER TABLE "AspNetUsers" ADD server_id uuid;

CREATE TABLE servers (
                         id uuid NOT NULL,
                         name character varying(25) NOT NULL,
                         image_path character varying(100),
                         description character varying(1000),
                         banner_image_path character varying(100),
                         CONSTRAINT pk_servers PRIMARY KEY (id)
);

CREATE TABLE server_invite_urls (
                                    id uuid NOT NULL,
                                    server_id uuid NOT NULL,
                                    uri_parameter character varying(20) NOT NULL,
                                    name character varying(50),
                                    amount_of_uses integer NOT NULL,
                                    uses integer NOT NULL,
                                    valid_till timestamp with time zone,
                                    CONSTRAINT pk_server_invite_urls PRIMARY KEY (id),
                                    CONSTRAINT fk_server_invite_urls_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE
);

CREATE TABLE server_members (
                                user_id uuid NOT NULL,
                                server_id uuid NOT NULL,
                                is_owner boolean NOT NULL,
                                CONSTRAINT pk_server_members PRIMARY KEY (user_id, server_id),
                                CONSTRAINT fk_server_members_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE,
                                CONSTRAINT fk_server_members_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
);

CREATE TABLE server_nodes (
                              id uuid NOT NULL,
                              name character varying(25) NOT NULL,
                              type text NOT NULL,
                              channel_topic character varying(500) NOT NULL,
                              is_private boolean NOT NULL,
                              is_age_restricted boolean NOT NULL,
                              parent_id uuid NOT NULL,
                              server_id uuid NOT NULL,
                              CONSTRAINT pk_server_nodes PRIMARY KEY (id),
                              CONSTRAINT fk_server_nodes_server_nodes_parent_id FOREIGN KEY (parent_id) REFERENCES server_nodes (id) ON DELETE CASCADE,
                              CONSTRAINT fk_server_nodes_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE
);

CREATE TABLE server_roles (
                              id uuid NOT NULL,
                              name character varying(25) NOT NULL,
                              description character varying(500),
                              is_default boolean NOT NULL,
                              server_id uuid NOT NULL,
                              permissions jsonb NOT NULL,
                              server_member_server_id uuid,
                              server_member_user_id uuid,
                              CONSTRAINT pk_server_roles PRIMARY KEY (id),
                              CONSTRAINT fk_server_roles_server_members_server_member_user_id_server_me FOREIGN KEY (server_member_user_id, server_member_server_id) REFERENCES server_members (user_id, server_id),
                              CONSTRAINT fk_server_roles_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id) ON DELETE CASCADE
);

CREATE INDEX ix_messages_receiver ON messages (receiver);

CREATE INDEX ix_messages_sender ON messages (sender);

CREATE INDEX ix_asp_net_users_server_id ON "AspNetUsers" (server_id);

CREATE INDEX ix_server_invite_urls_server_id ON server_invite_urls (server_id);

CREATE INDEX ix_server_invite_urls_uri_parameter ON server_invite_urls (uri_parameter);

CREATE INDEX ix_server_members_server_id ON server_members (server_id);

CREATE INDEX ix_server_nodes_parent_id ON server_nodes (parent_id);

CREATE INDEX ix_server_nodes_server_id ON server_nodes (server_id);

CREATE INDEX ix_server_roles_server_id ON server_roles (server_id);

CREATE INDEX ix_server_roles_server_member_user_id_server_member_server_id ON server_roles (server_member_user_id, server_member_server_id);

ALTER TABLE "AspNetUsers" ADD CONSTRAINT fk_asp_net_users_servers_server_id FOREIGN KEY (server_id) REFERENCES servers (id);
