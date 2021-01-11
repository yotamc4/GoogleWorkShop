import * as React from "react";
import { Nav, INavLinkGroup } from "office-ui-fabric-react/lib/Nav";
import { INavLink, Label, Stack, Text } from "@fluentui/react";
import { CategoriesMap } from "../Model/Categories";
import { boldStyle } from "../../TextStyles";
import { useHistory } from "react-router-dom";

function createINavLinkFromString(name: string, parentName: string): INavLink {
  return {
    name: name,
    url: createUri(parentName, name),
  };
}

const createUri = (category: string, subCategory?: string): string => {
  const url: URL = new URL("/groups", window.location.origin);
  const newUrlParams = new URLSearchParams();

  const currentSearchParams: URLSearchParams = new URLSearchParams(
    window.location.search
  );

  newUrlParams.set("category", category);

  if (subCategory) {
    newUrlParams.set("subCategory", subCategory);
  }

  return url.href + "?" + newUrlParams.toString();
};

function navLinkGroups(map: Map<string, string[]>): INavLinkGroup[] {
  let links: INavLink[] = [];
  map.forEach((value: string[], key: string) => {
    links.push({
      name: key,
      url: createUri(key),
      onClick: () => {},
      links: value.map((value) => createINavLinkFromString(value, key)),
    });
  });

  return [
    {
      links: links,
    },
  ];
}

export const NavigationPane: React.FunctionComponent = () => {
  const history = useHistory();
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
        onLinkClick={(ev?: React.MouseEvent<HTMLElement>, item?: INavLink) =>
          history.push(item?.link)
        }
      />
    </Stack>
  );
};
