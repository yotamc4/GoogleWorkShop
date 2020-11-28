import * as React from "react";
import { Nav, INavLinkGroup } from "office-ui-fabric-react/lib/Nav";
import { INavLink, Label, Stack, Text } from "@fluentui/react";
import { CategoriesMap } from "../Model/Categories";
import { boldStyle } from "../../TextStyles";

function createINavLinkFromString(name: string): INavLink {
  return {
    name: name,
    url: `http://example.com/${name}`,
    target: "_blank",
  };
}

function navLinkGroups(map: Map<string, string[]>): INavLinkGroup[] {
  let links: INavLink[] = [];
  map.forEach((value: string[], key: string) => {
    links.push({
      name: key,
      url: `http://example.com/${key}`,
      target: "_blank",
      links: value.map(createINavLinkFromString),
    });
  });

  return [
    {
      links: links,
    },
  ];
}

export const NavigationPane: React.FunctionComponent = () => {
  return (
    <Stack>
      <Label>
        <Text variant="large" styles={boldStyle}>
          Categories
        </Text>
      </Label>
      <Nav
        ariaLabel="Nav with nested links"
        groups={navLinkGroups(CategoriesMap)}
      />
    </Stack>
  );
};
